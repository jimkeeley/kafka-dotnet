namespace JOAT.Kafka.RatingsConsumer;

public class Startup
{
    public void ConfigureServices(IServiceCollection collection)
    {
        //use secrets file for local dev
        //use environment variables for cloud deployment
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        var kafkaOptions = configuration
            .GetRequiredSection(nameof(KafkaOptions))
            .Get<KafkaOptions>();

        collection.AddSingleton<IConfiguration>(configuration);

        collection.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>(provider => new SqlConnectionFactory(configuration["SqlConnectionString"]));
        collection.AddSingleton<IRankingsRepository, RankingsRepository>();
        collection.Configure<KafkaOptions>(configuration.GetSection(key: nameof(KafkaOptions)));

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
                            .CreateTopicIfNotExists(kafkaOptions.TopicName, kafkaOptions.NumberOfPartitions,
                                kafkaOptions.ReplicationFactor)
                            .AddConsumer(consumer => consumer
                                .Topic(kafkaOptions.TopicName)
                                .WithGroupId(kafkaOptions.GroupId)
                                .WithBufferSize(100)
                                .WithWorkersCount(3)
                                .WithAutoOffsetReset(AutoOffsetReset.Latest)
                                .AddMiddlewares(
                                    middlewares => middlewares
                                        .AddDeserializer<MyJsonCoreDeserializer>()
                                        .AddTypedHandlers(h => h.AddHandler<RankingPersistenceMessageHandler>())
                                ))
                            .EnableAdminMessages("kafka-flow.admin")
                            .EnableTelemetry("kafka-flow.admin");
                    }).AddOpenTelemetryInstrumentation()
        ).AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        app
            .UseRouting()
            .UseEndpoints(endpoints => { endpoints.MapControllers(); })
            .UseKafkaFlowDashboard();
    }
}