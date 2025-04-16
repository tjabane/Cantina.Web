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
        Task<List<MenuItemView>> GetAllMenuItemsAsync();
        Task AddMenuItemAsync(MenuItemView menuItem);
        Task UpdateMenuItemAsync(MenuItemView menuItem);
        Task DeleteMenuItemAsync(int id);
    }
}
