using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IMenuRepository MenuRepository { get; }
        IMenuAuditRepository MenuAuditRepository { get; }
        IReviewRepository ReviewRepository { get; }
        Task SaveChangesAsync();
    }
}
