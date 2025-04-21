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
    public class MenuRepository(CantinaDbContext context) : IMenuRepository, IDisposable
    {
        private readonly CantinaDbContext _context = context;

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

        public Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MenuItem> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MenuItem>> SearchAsync(string name)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
