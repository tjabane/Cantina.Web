using Cantina.Core.Dto;
using Cantina.Core.Interface;
using NRedisStack.RedisStackCommands;
using NRedisStack;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NRedisStack.Search;
using System.Text.Json;

namespace Cantina.Infrastructure.Redis
{
    public class MenuQueryRepository(IConnectionMultiplexer redis) : IMenuQueryRepository
    {
        private readonly string _indexName = "menu-index";
        private readonly JsonCommands _jsonCMD = redis.GetDatabase().JSON();
        private readonly SearchCommands _searchCommands = redis.GetDatabase().FT();

        public async Task<List<MenuItem>> GetAllAsync()
        {
            var menuResponse = await _searchCommands.SearchAsync(_indexName, new Query("*"));
            return [.. menuResponse.ToJson().Select(json => JsonSerializer.Deserialize<MenuItem>(json))];
        }

        public async Task<MenuItem> GetByIdAsync(int id)
        {
            return await _jsonCMD.GetAsync<MenuItem>($"menu:{id}");
        }

        public async Task<List<MenuItem>> SearchAsync(string searchTerm)
        {
            var menuResponse = await _searchCommands.SearchAsync(_indexName, new Query("*"));
            var filtered = menuResponse.ToJson().Select(json => JsonSerializer.Deserialize<MenuItem>(json))
                                                .Where(item => item.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            return [.. filtered];
        }
    }
}
