using Cantina.Application.UseCase.Menu.Query.GetMenu;
using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Menu.Query.SearchMenu
{
    public class SearchMenuItemQueryHandler(IMenuRepository menuRepository) : IRequestHandler<SearchMenuItemQuery, Result<List<MenuItem>>>
    {
        private readonly IMenuRepository _menuRepository = menuRepository;

        public async Task<Result<List<MenuItem>>> Handle(SearchMenuItemQuery request, CancellationToken cancellationToken)
        {
            var searchResults = await _menuRepository.SearchAsync(request.Search);
            if (searchResults == null || searchResults.Count == 0)
                return Result.Fail(new Error("No menu items found"));
            return Result.Ok(searchResults);
        }
    }
}
