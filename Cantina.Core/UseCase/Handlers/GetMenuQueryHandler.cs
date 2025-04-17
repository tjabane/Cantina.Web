using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests;
using Cantina.Core.UseCase.Requests.Queries;
using FluentResults;
using MediatR;

namespace Cantina.Core.UseCase.Handlers
{
    public class GetMenuQueryHandler(IMenuItemRepository menuItemRepository) : IRequestHandler<GetMenuQuery, Result<List<MenuItem>>>
    {
        private readonly IMenuItemRepository _menuItemRepository = menuItemRepository;

        public async Task<Result<List<MenuItem>>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            var menuItems = await _menuItemRepository.GetAllAsync();
            if (menuItems.Count == 0)
                return Result.Fail("No menu found");
            return Result.Ok(menuItems);
        }
    }
}
