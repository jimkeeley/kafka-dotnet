using System;
using System.Threading.Tasks;
using JOAT.Kafka.Consumer;
using JOAT.Kafka.Domain.Configuration;
using JOAT.Kafka.Domain.Messages;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Producers;
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

var kafkaOptions = configuration
    .GetSection(nameof(KafkaOptions))
    .Get<KafkaOptions>();

builder.ConfigureServices(collection => {
    collection.AddKafka(
        kafka => kafka
            .UseConsoleLog()
            .AddCluster(
                cluster =>
                {
                    cluster
                        .WithBrokers(new[] { kafkaOptions.Server }).WithSecurityInformation(information =>
                        {
                            information.SecurityProtocol = SecurityProtocol.SaslSsl;
                            information.SaslMechanism = SaslMechanism.Plain;
                            information.SaslUsername = kafkaOptions.ApiKey;
                            information.SaslPassword = kafkaOptions.ApiSecret;
                        })
                        .CreateTopicIfNotExists(kafkaOptions.TopicName, 6, 3)
                        .AddProducer(
                            "JOAT-Producer",
                            producer => producer
                                .DefaultTopic(kafkaOptions.TopicName)
                                .AddMiddlewares(m => m.AddSerializer<ProtobufNetSerializer>())
                        );
                })
    );
});



using IHost host = builder.Build();

var serviceProvider = host.Services.GetService<IServiceProvider>();

//var bus = serviceProvider.CreateKafkaBus();

//await bus.StartAsync();

var producer = serviceProvider
    .GetRequiredService<IProducerAccessor>()
    .GetProducer("JOAT-Producer");

Console.WriteLine("Type the number of messages to produce or 'exit' to quit:");

while (true)
{
    var input = Console.ReadLine();

    if (int.TryParse(input, out var count))
    {
        for (var i = 0; i < count; i++)
        {
            await producer.ProduceAsync(
                kafkaOptions.TopicName,
                Guid.NewGuid().ToString(),
                new TestMessage { Text = $"Message: {Guid.NewGuid()}" });
        }
    }

    if (input!.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        //await bus.StopAsync();
        break;
    }
}

await Task.Delay(3000);
