using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using FluentResults;
using MediatR;

namespace Cantina.Application.UseCase.Menu.Query.GetMenu
{
    public class GetMenuQueryHandler(IMenuRepository menuRepository) : IRequestHandler<GetMenuQuery, Result<List<MenuItem>>>
    {
        private readonly IMenuRepository _menuRepository = menuRepository;
        public async Task<Result<List<MenuItem>>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            var menuItems = await _menuRepository.GetAllAsync();
            var activeMenuItems = menuItems.Where(x => !x.IsDeleted).ToList();
            return (activeMenuItems.Count == 0) ?  Result.Fail(new Error("No active menu items found.")): Result.Ok(activeMenuItems);
        }
    }
}
