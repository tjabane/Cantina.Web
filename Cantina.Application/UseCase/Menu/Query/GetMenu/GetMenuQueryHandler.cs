using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using FluentResults;
using MediatR;

namespace Cantina.Application.UseCase.Menu.Query.GetMenu
{
    public class GetMenuQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMenuQuery, Result<List<MenuItem>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<List<MenuItem>>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            var menuItems = await _unitOfWork.MenuRepository.GetAllAsync();
            var activeMenuItems = menuItems.Where(x => !x.IsDeleted).ToList();
            return (activeMenuItems.Count == 0) ?  Result.Fail("No active menu items found."): Result.Ok(activeMenuItems);
        }
    }
}
