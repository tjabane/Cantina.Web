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
    public class UpdateMenuItemCommandHandler(IMenuCommandRepository menuCmdRepository, IMenuQueryRepository menuQueryRepository) : IRequestHandler<UpdateMenuItemCommand, Result<MenuItem>>
    {
        private readonly IMenuCommandRepository _menuCmdRepository = menuCmdRepository;
        private readonly IMenuQueryRepository _menuQueryRepository = menuQueryRepository;

        public async Task<Result<MenuItem>> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = await _menuQueryRepository.GetByIdAsync(request.Id);
            if (menuItem is null)
                return Result.Fail(new Error($"Menu item with ID {request.Id} not found"));
            menuItem.Name = request.MenuItem.Name;
            menuItem.Description = request.MenuItem.Description;
            menuItem.Price = request.MenuItem.Price;
            menuItem.Image = request.MenuItem.Image;
            await _menuCmdRepository.UpdateAsync(menuItem);
            return Result.Ok(menuItem);
        }
    }
}
