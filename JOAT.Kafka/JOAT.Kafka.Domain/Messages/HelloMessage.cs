using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JOAT.Kafka.Domain.Messages
{
    public class HelloMessage
    {
        public string Text { get; set; } = default!;
    }

    [DataContract]
    public class TestMessage
    {
        [DataMember(Order = 1)]
        public string Text { get; set; }
    }
}