using Cantina.Core.Data.Entities;
using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cantina.Infrastructure.MessageBroker
{
    public class ReviewCommandRepository : IReviewCommandRepository
    {
        private readonly string _topic;
        private readonly IProducer<Null, string> _producer;
        public ReviewCommandRepository(string host, string topic)
        {
            _topic = topic;
            var producerConfig = new ProducerConfig { BootstrapServers = host };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }
        public async Task AddAsync(Core.Dto.Review review)
        {
            var message = JsonSerializer.Serialize(review);
            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
        }
    }
}
