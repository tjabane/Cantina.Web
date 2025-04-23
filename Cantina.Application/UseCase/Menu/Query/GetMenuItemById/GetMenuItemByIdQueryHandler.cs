using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Menu.Query.GetMenuItemById
{
    public class GetMenuItemByIdQueryHandler(IMenuRepository menuRepository) : IRequestHandler<GetMenuItemByIdQuery, Result<MenuItem>>
    {
        private readonly IMenuRepository _menuRepository = menuRepository;

        public async Task<Result<MenuItem>> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
        {
            var menuItem = await _menuRepository.GetByIdAsync(request.Id);
            if (menuItem is null || menuItem.IsDeleted)
                return Result.Fail(new Error($"Menu item with ID {request.Id} not found"));
            return Result.Ok(menuItem);
        }
    }
}
