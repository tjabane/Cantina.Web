using Cantina.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Domain.Repositories
{
    public interface IMenuRepository
    {
        Task<List<MenuItem>> GetAllAsync();
        Task<MenuItem> GetByIdAsync(int id);
        Task<IEnumerable<MenuItem>> SearchAsync(string name);
        Task AddAsync(MenuItem menuItem);
        void UpdateAsync(MenuItem menuItem);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
