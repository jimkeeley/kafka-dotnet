// Global using directives

global using Confluent.Kafka;
global using JOAT.IFPA.ApiClient;
global using JOAT.Kafka.Domain.Configuration;
global using JOAT.Kafka.Domain.Messages;
global using KafkaFlow;
global using KafkaFlow.Configuration;
global using KafkaFlow.Producers;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using SaslMechanism = KafkaFlow.Configuration.SaslMechanism;
global using SecurityProtocol = KafkaFlow.Configuration.SecurityProtocol;