using Cantina.Application.UseCase.Menu.Commands.AddMenuItem;
using Cantina.Application.UseCase.Menu.Commands.UpdateMenuItem;
using Cantina.Web.Dto;

namespace Cantina.Web.Helpers
{
    public static class CommandFactory
    {
        public static AddMenuItemCommand CreateAddMenuItemCommand(MenuItemDto menuItem, string userId)
        {
            return new AddMenuItemCommand
            {
                UserId = userId,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                ImageUrl = menuItem.ImageUrl,
                Type = menuItem.Type
            };
        }

        public static UpdateMenuItemCommand CreateUpdateMenuItemCommand(MenuItemDto menuItem, int itemId, string userId)
        {
            return new UpdateMenuItemCommand
            {
                Id = itemId,
                UserId = userId,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                ImageUrl = menuItem.ImageUrl,
                Type = menuItem.Type
            };
        }
    }
}
