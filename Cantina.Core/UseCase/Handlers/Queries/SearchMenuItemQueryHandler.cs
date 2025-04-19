using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests.Queries;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Handlers.Queries
{
    public class SearchMenuItemQueryHandler(IMenuQueryRepository menuQueryRepository) : IRequestHandler<SearchMenuItemQuery, Result<List<MenuItem>>>
    {
        private readonly IMenuQueryRepository _menuQueryRepository = menuQueryRepository;
        public async Task<Result<List<MenuItem>>> Handle(SearchMenuItemQuery request, CancellationToken cancellationToken)
        {
            var searchResults = await _menuQueryRepository.SearchAsync(request.SearchTerm);
            return Result.Ok(searchResults);
        }
    }
}
