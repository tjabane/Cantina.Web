using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Confluent.Kafka;
using System.Text.Json;

namespace Cantina.Infrastructure.MessageBroker
{
    public class MessageProducerClient : IMenuCommandRepository
    {
        private readonly string _topic;
        private readonly IProducer<int, string> _producer; 

        public MessageProducerClient(string host, string topic)
        {
            _topic = topic;
            var producerConfig = new ProducerConfig { BootstrapServers = host };
            _producer = new ProducerBuilder<int, string>(producerConfig).Build();
        }

        public async Task AddAsync(MenuItem menuItem)
        {
            var message = JsonSerializer.Serialize(menuItem);
            await _producer.ProduceAsync(_topic, new Message<int, string> { Key = menuItem.Id, Value = message });
        }
        public async Task UpdateAsync(MenuItem menuItem)
        {
            var message = JsonSerializer.Serialize(menuItem);
            await _producer.ProduceAsync(_topic, new Message<int, string> { Key = menuItem.Id, Value = message });
        }

        public async Task DeleteAsync(int id)
        {
            var delete = JsonSerializer.Serialize(new { delete = true, id });
            await _producer.ProduceAsync(_topic, new Message<int, string> { Key = id, Value = delete });
        }

        
    }
}
