using JOAT.Kafka.Consumer;
using JOAT.Kafka.Domain.Configuration;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConsole();
});

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

builder.ConfigureServices(collection =>
{
    collection.AddSingleton<IConfiguration>(configuration);
    collection.Configure<KafkaOptions>(
        configuration.GetSection(key: nameof(KafkaOptions)));

    collection.AddKafka(
        kafka => kafka
            .UseConsoleLog()
            .AddCluster(
                cluster =>
                {
                    var kafkaOptions = configuration
                        .GetSection(nameof(KafkaOptions))
                        .Get<KafkaOptions>();

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
                                    .AddDeserializer<ProtobufNetDeserializer>()
                                    .AddTypedHandlers(h => h.AddHandler<HelloMessageHandler>())
                            ));
                })
    );
});


using IHost host = builder.Build();

var serviceProvider = host.Services.GetService<IServiceProvider>();

var bus = serviceProvider.CreateKafkaBus();

await bus.StartAsync();

Console.Read();


