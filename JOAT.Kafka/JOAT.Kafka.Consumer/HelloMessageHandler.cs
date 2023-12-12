using KafkaFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOAT.Kafka.Domain.Messages;
using System.Text.Json;

namespace JOAT.Kafka.Consumer
{
    public class HelloMessageHandler : IMessageHandler<TestMessage>
    {
        public Task Handle(IMessageContext context, TestMessage message)
        {
            Console.WriteLine(
                "Partition: {0} | Offset: {1} | Message: {2}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,
                message.Text);

            return Task.CompletedTask;
        }
    }

}
