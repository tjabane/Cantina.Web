using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Handlers
{
    public class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, Result<MenuItem>>
    {
        private readonly IMenuItemRepository _menuItemRepository;
        public CreateMenuItemCommandHandler(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }
        public async Task<Result<MenuItem>> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = request.MenuItem;
            await _menuItemRepository.AddMenuItemAsync(menuItem);
            return Result.Ok();
        }
    }
    
}
