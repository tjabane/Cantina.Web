using Cantina.Domain.Entities;
using FluentResults;
using Ganss.Xss;
using MediatR;


namespace Cantina.Application.UseCase.Menu.Query.SearchMenu
{
    public class SearchMenuItemQuery : IRequest<Result<List<MenuItem>>>
    {
        private readonly HtmlSanitizer _sanitizer = new();
        public SearchMenuItemQuery(string search) 
        {
            Search = _sanitizer.Sanitize(search);
        }
        public string Search { get; set; }
    }
}
