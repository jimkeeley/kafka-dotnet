using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOAT.Kafka.Domain.Configuration
{
    public class KafkaOptions
    {
        public const string Kafka = "Kafka";

        public string? Server { get; set; }

        public string? ApiSecret { get; set; }

        public string? ApiKey { get; set; }

        public int NumberOfPartitions { get; set; }

        public short ReplicationFactor { get; set; }

        public string? TopicName { get; set; }

        public string? GroupId { get; set; }

    }
}
