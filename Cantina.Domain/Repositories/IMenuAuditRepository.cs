using Cantina.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Domain.Repositories
{
    public interface IMenuAuditRepository
    {
        Task AddAsync(MenuAudit menuAudit);
        Task SaveChangesAsync();
    }
}
