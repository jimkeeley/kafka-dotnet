//This producer connects to Kafka cluster
//reads all the rankings from the IFPA Pinball Api
//foreach ranking record, we put an associated
//message into kafka topic

var builder = Host.CreateDefaultBuilder(args);

//TODO: Add OpenTelemetry (for App Insights, Grafana, etc)
builder.ConfigureLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConsole();
});

//use secrets file for local dev
//use environment variables for cloud deployment
var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>(true)
    .AddEnvironmentVariables()
    .Build();

//retrieve kafka config 
var kafkaOptions = configuration
    .GetRequiredSection(nameof(KafkaOptions))
    .Get<KafkaOptions>();

//retrieve api config
var ifpaApiOptions = configuration
    .GetRequiredSection(nameof(IFPAApiOptions))
    .Get<IFPAApiOptions>();

builder.ConfigureServices(collection =>
{
    //register api client and related dependencies with DI
    collection.AddScoped<IIFPAApiClient, IFPAApiClient>();
    collection.AddHttpClient<IIFPAApiClient, IFPAApiClient>(client =>
    {
        client.BaseAddress = new Uri(ifpaApiOptions.ApiBaseUrl);
    });
    collection.AddScoped(_ => ifpaApiOptions);
    collection.Configure<IFPAApiOptions>(configuration.GetSection(key: nameof(ifpaApiOptions)));

    //setup Kafka
    collection.AddKafkaFlowHostedService(
        kafka => kafka
            .UseMicrosoftLog()
            .AddCluster(
                cluster =>
                {
                    //cluster in this case is confluent cloud
                    cluster
                        .WithBrokers(new[] { kafkaOptions.Server })
                        .WithSecurityInformation(information =>
                        {
                            information.SecurityProtocol = SecurityProtocol.SaslSsl;
                            information.SaslMechanism = SaslMechanism.Plain;
                            information.SaslUsername = kafkaOptions.ApiKey;
                            information.SaslPassword = kafkaOptions.ApiSecret;
                        })
                        .CreateTopicIfNotExists(kafkaOptions.TopicName, kafkaOptions.NumberOfPartitions, kafkaOptions.ReplicationFactor)
                        .AddProducer(
                            kafkaOptions.ProducerName,
                            producer => producer
                                //.WithCompression(CompressionType.Gzip)
                                .DefaultTopic(kafkaOptions.TopicName)
                                .AddMiddlewares(m => m.AddSerializer<MyJsonCoreSerializer>())
                        );
                }).AddOpenTelemetryInstrumentation()
    );
});


using IHost host = builder.Build();
var serviceProvider = host.Services.GetRequiredService<IServiceProvider>();

var producer = serviceProvider
    .GetRequiredService<IProducerAccessor>()
    .GetProducer(kafkaOptions!.ProducerName);

//resolve api client from DI Container
var ifpaApiClient = host.Services.GetRequiredService<IIFPAApiClient>();

//create dictionary to store all the ranking results
//use dictionary so we can ensure we don't get any dupes
var rankings = new Dictionary<string,Ranking>();

//Api call limits results via paging
//max count is 500 so we need to get 500 results at a time until we hit the end
//make initial call with position 1
var rankingRequest = new RankingsRequest()
{
    Count = 500,
    Order = "points",
    StartPosition = 1
};
var results = await ifpaApiClient.GetRankingsAsync(rankingRequest);

//Add results to dictionary
foreach (var result in results.Rankings)
{
    if (!rankings.ContainsKey(result.PlayerId))
    {
        rankings.Add(result.PlayerId, result);

        //send message to Kafka for each rating
        await producer.ProduceAsync(
            kafkaOptions.TopicName,
            Guid.NewGuid().ToString(),
            result);
    }
}

rankingRequest.StartPosition = 501;

var count = 0;
while (count < 100)//we shouldn't ever hit one hundred as the loop will break when we hit the last page, but just in case
{
    //appears to be some sort of throttling where null is returned
    //on occasion
    //TODO:Use Polly and exponential Backup Policy for retries
    if (results.Rankings == null)
    {
        await Task.Delay(10000);
    }

    try
    {

        results = await ifpaApiClient.GetRankingsAsync(rankingRequest);
    }
    catch (HttpRequestException e)
    {
        //when we get to the last page
        //api will return a 400
        //use this to break out of the loop
        //note the total count that is returned in the response is not accurate 
        //so we have no way of knowing when we reached the last page, not ideal, but no control over api
        if (e.Message.IndexOf("Response status code does not indicate success: 400 (Bad Request).") >= 1)
        {
            break;
        }

        throw;
    }

    //send message to Kafka for each rating
    foreach (var result in results.Rankings)
    {
        if (!rankings.ContainsKey(result.PlayerId))
        {
            rankings.Add(result.PlayerId, result);
            await producer.ProduceAsync(
                kafkaOptions.TopicName,
                Guid.NewGuid().ToString(),
                result);
        }
    }
    //increment page position
    rankingRequest.StartPosition += 500;

    await Task.Delay(2000);//avoid hitting rate limit
    count++;
}


Console.Read();
