using Cantina.Domain.Repositories;
using Cantina.Infrastructure.Database;
using Cantina.Infrastructure.Options;
using Microsoft.Extensions.Options;
using StackExchange.Redis;


namespace Cantina.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CantinaDbContext _context;
        public UnitOfWork(CantinaDbContext context, IConnectionMultiplexer redis, IOptions<RedisOptions> redisOptions)
        {
            _context = context;
            MenuRepository = new MenuRepository(_context, redis, redisOptions);
            MenuAuditRepository = new MenuAuditRepository(_context);
            //ReviewRepository = new ReviewRepository(_context);
        }
        public IMenuRepository MenuRepository { get; }
        public IMenuAuditRepository MenuAuditRepository { get; }
        public IReviewRepository ReviewRepository { get; }

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
