using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests;
using Cantina.Core.UseCase.Requests.Queries;
using FluentResults;
using MediatR;

namespace Cantina.Core.UseCase.Handlers
{
    public class GetMenuQueryHandler(IMenuQueryRepository menuQueryRepository) : IRequestHandler<GetMenuQuery, Result<List<MenuItem>>>
    {
        private readonly IMenuQueryRepository _menuQueryRepository = menuQueryRepository;
        public async Task<Result<List<MenuItem>>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            var menuItems = await _menuQueryRepository.GetAllAsync();
            if(menuItems == null || menuItems.Count == 0)
                return Result.Fail("No menu items found");
            return Result.Ok(menuItems);
        }
    }
}
