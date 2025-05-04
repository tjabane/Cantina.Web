using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using Cantina.Infrastructure.Database;
using Cantina.Infrastructure.Metrics;
using Cantina.Infrastructure.Options;
using Microsoft.Extensions.Options;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using StackExchange.Redis;
using System.Text.Json;

namespace Cantina.Infrastructure.Repository
{
    public class ReviewRepository(CantinaDbContext context, IConnectionMultiplexer redis, IOptions<RedisOptions> redisOptions, ReviewsMeter reviewsMeter) : IReviewRepository
    {
        private readonly ReviewsMeter _reviewsMeter = reviewsMeter;
        private readonly CantinaDbContext _context = context;
        private readonly string _indexName = redisOptions.Value.ReviewIndex;
        private readonly SearchCommands _searchCommands = redis.GetDatabase().FT();
        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            _reviewsMeter.IncrementReviewsAddedCounter();
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            var menuResponse = await _searchCommands.SearchAsync(_indexName, new Query("*"));
            return [.. menuResponse.ToJson().Select(json => JsonSerializer.Deserialize<Review>(json))];
        }
    }
}
