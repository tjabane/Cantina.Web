using Cantina.Core.Dto;
using Cantina.Core.Interface;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cantina.Infrastructure.Redis
{
    public class ReviewQueryRepository(IConnectionMultiplexer redis) : IReviewQueryRepository
    {
        private readonly string _indexName = "reviews-index";
        private readonly SearchCommands _searchCommands = redis.GetDatabase().FT();
        public async Task<List<ReviewViewDto>> GetAllAsync()
        {
            var menuResponse = await _searchCommands.SearchAsync(_indexName, new Query("*"));
            return [.. menuResponse.ToJson().Select(json => JsonSerializer.Deserialize<ReviewViewDto>(json))];
        }
    }
}
