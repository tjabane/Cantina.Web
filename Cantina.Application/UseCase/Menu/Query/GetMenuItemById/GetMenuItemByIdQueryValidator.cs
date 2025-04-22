using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Menu.Query.GetMenuItemById
{
    public class GetMenuItemByIdQueryValidator: AbstractValidator<GetMenuItemByIdQuery>
    {
        public GetMenuItemByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Menu item ID must not be empty.")
                .GreaterThan(0)
                .WithMessage("Menu item ID must be greater than 0.");
        }
    }
}
