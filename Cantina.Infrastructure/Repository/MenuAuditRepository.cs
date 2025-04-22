using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using Cantina.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Infrastructure.Repository
{
    public class MenuAuditRepository(CantinaDbContext cantinaDbContext) : IMenuAuditRepository
    {
        private readonly CantinaDbContext _context = cantinaDbContext;
        public async Task AddAsync(MenuAudit menuAudit)
        {
            await _context.MenuAudits.AddAsync(menuAudit);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
