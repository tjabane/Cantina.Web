using Cantina.Core.Dto;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Requests.Queries
{
    public class SearchMenuItemQuery(string searchTerm) : IRequest<Result<List<MenuItem>>>
    {
        private readonly string _searchTerm = searchTerm;
        public string SearchTerm => _searchTerm;
    }
}
