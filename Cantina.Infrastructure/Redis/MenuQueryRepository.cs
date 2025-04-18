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
    public class MenuQueryRepository : IMenuQueryRepository
    {
        private readonly JsonCommands _jsonCMD;
        private readonly SearchCommands _searchCommands;
        public MenuQueryRepository(IConnectionMultiplexer redis)
        {
            _searchCommands = redis.GetDatabase().FT();
            _jsonCMD = redis.GetDatabase().JSON();
        }
        public async Task<List<MenuItem>> GetAllAsync()
        {
            var menuResponse = await _searchCommands.SearchAsync("menu-index", new Query("*"));
            return [.. menuResponse.ToJson().Select(json => JsonSerializer.Deserialize<MenuItem>(json))];
        }

        public async Task<MenuItem> GetByIdAsync(int id)
        {
            return await _jsonCMD.GetAsync<MenuItem>($"menu:{id}");
        }
    }
}
