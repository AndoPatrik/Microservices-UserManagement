using Confluent.Kafka;
using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public class UsermanagementProducer : IUsermanagementProducer
    {
        public void Produce(string msg)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092,kafka:9093" };

            using (var producer = new ProducerBuilder<Null,string>(config).Build())
            {
                producer.ProduceAsync("usermanagement",new Message<Null, string> {Value = msg});

                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}
