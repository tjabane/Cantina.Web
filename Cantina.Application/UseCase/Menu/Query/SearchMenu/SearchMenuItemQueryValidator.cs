using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Menu.Query.SearchMenu
{
    public class SearchMenuItemQueryValidator: AbstractValidator<SearchMenuItemQuery>
    {
        public SearchMenuItemQueryValidator()
        {
            RuleFor(x => x.Search)
                .NotEmpty()
                .WithMessage("Search term cannot be empty");
        }
    }
}
