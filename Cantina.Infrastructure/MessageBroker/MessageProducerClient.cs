using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cantina.Infrastructure.MessageBroker
{
    public class MessageProducerClient : IMenuCommandRepository
    {
        private readonly IProducer<int, string> _producer;
        public MessageProducerClient(IConfiguration configuration)
        {
            var producerconfig = new ProducerConfig
            {
                BootstrapServers = configuration["MessageBroker:Server"]
            };
            _producer = new ProducerBuilder<int, string>(producerconfig).Build();
        }

        public async Task AddAsync(MenuItem menuItem)
        {
            var message = JsonSerializer.Serialize(menuItem);
            await _producer.ProduceAsync("menu-items", new Message<int, string>
            {
                Key = menuItem.Id,
                Value = message
            });
        }

        public Task DeleteAsync(int id)
        {
            var delete = JsonSerializer.Serialize(new { delete = true, id });
            return _producer.ProduceAsync("menu-items", new Message<int, string>
            {
                Key = id,
                Value = delete
            });
        }

        public async Task UpdateAsync(MenuItem menuItem)
        {
            var message = JsonSerializer.Serialize(menuItem);
            await _producer.ProduceAsync("menu-items", new Message<int, string>
            {
                Key = menuItem.Id,
                Value = message
            });
        }
    }
}
