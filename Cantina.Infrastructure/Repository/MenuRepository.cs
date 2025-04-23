using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using Cantina.Infrastructure.Database;
using NRedisStack.RedisStackCommands;
using NRedisStack;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using NRedisStack.Search;
using Microsoft.Extensions.Options;
using Cantina.Infrastructure.Options;

namespace Cantina.Infrastructure.Repository
{
    public class MenuRepository(CantinaDbContext context, IConnectionMultiplexer redis, IOptions<RedisOptions> redisOptions) : IMenuRepository, IDisposable
    {
        private readonly CantinaDbContext _context = context;
        private readonly string _indexName = redisOptions.Value.MenuIndex;
        private readonly JsonCommands _jsonCMD = redis.GetDatabase().JSON();
        private readonly SearchCommands _searchCommands = redis.GetDatabase().FT();

        public async Task AddAsync(MenuItem menuItem)
        {
            await _context.MenuItems.AddAsync(menuItem);
        }

        public void UpdateAsync(MenuItem menuItem)
        {
            _context.Update(menuItem);
        }

        public async Task DeleteAsync(int id)
        {
            var menu = await _context.MenuItems.FindAsync(id);
            menu.IsDeleted = true;
            _context.MenuItems.Update(menu);
        }

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

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
