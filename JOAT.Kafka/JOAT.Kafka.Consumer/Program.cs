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
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var kafkaOptions = configuration
    .GetRequiredSection(nameof(KafkaOptions))
    .Get<KafkaOptions>();

builder.ConfigureServices(collection =>
{
    collection.AddSingleton<IConfiguration>(configuration);
    collection.Configure<KafkaOptions>(configuration.GetSection(key: nameof(KafkaOptions)));

    //setup Kafka
    collection.AddKafka(
        kafka => kafka
            .UseConsoleLog()
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
                        .CreateTopicIfNotExists(kafkaOptions.TopicName, kafkaOptions.NumberOfPartitions,
                            kafkaOptions.ReplicationFactor)
                        .AddConsumer(consumer => consumer
                            .Topic(kafkaOptions.TopicName)
                            .WithGroupId("test")
                            .WithBufferSize(100)
                            .WithWorkersCount(3)
                            .AddMiddlewares(
                                middlewares => middlewares
                                    .AddDeserializer<JsonCoreDeserializer>()
                                    .AddTypedHandlers(h => h.AddHandler<RankingPersistenceMessageHandler>())
                            ));
                })
    );
});


using IHost host = builder.Build();

var serviceProvider = host.Services.GetService<IServiceProvider>();

//need to resolve and start Message bus For Consumer
var bus = serviceProvider.CreateKafkaBus();

await bus.StartAsync();

//block console from exiting so consumer remains 'listening'
Console.Read();



