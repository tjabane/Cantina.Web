using Cantina.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.Interface
{
    public interface IMenuItemRepository
    {
        Task<List<MenuItem>> GetAllMenuItemsAsync();
        Task<MenuItem> GetItemByIdAsync(int id);
        Task AddMenuItemAsync(MenuItem menuItem);
        Task UpdateMenuItemAsync(MenuItem menuItem);
        Task DeleteMenuItemAsync(int id);
    }
}
