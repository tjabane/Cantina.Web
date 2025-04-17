using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests;
using Cantina.Core.UseCase.Requests.Queries;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Handlers
{
    public class GetMenuItemByIdQueryHandler(IMenuItemRepository menuItemRepository) : IRequestHandler<GetMenuItemByIdQuery, Result<MenuItem>>
    {
        private readonly IMenuItemRepository _menuItemRepository = menuItemRepository;

        public async Task<Result<MenuItem>> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(request.ItemId);
            if (menuItem is null)
                return Result.Fail(new Error($"Menu item with ID {request.ItemId} not found"));
            return Result.Ok(menuItem);
        }
    }
}
