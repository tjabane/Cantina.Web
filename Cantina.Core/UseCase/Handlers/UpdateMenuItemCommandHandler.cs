using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests;
using Cantina.Core.UseCase.Requests.Commands;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Handlers
{
    public class UpdateMenuItemCommandHandler(IMenuItemRepository menuItemRepository) : IRequestHandler<UpdateMenuItemCommand, Result<MenuItem>>
    {
        private readonly IMenuItemRepository _menuItemRepository = menuItemRepository;

        public async Task<Result<MenuItem>> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(request.Id);
            if (menuItem is null)
                return Result.Fail(new Error($"Menu item with ID {request.MenuItem.Id} not found"));
            menuItem.Name = request.MenuItem.Name;
            menuItem.Description = request.MenuItem.Description;
            menuItem.Price = request.MenuItem.Price;
            menuItem.Image = request.MenuItem.Image;
            await _menuItemRepository.UpdateAsync(menuItem);
            return Result.Ok(menuItem);
        }
    }
}
