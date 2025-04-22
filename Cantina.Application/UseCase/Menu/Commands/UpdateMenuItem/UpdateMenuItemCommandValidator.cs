using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Menu.Commands.UpdateMenuItem
{
    public class UpdateMenuItemCommandValidator: AbstractValidator<UpdateMenuItemCommand>
    {
        public UpdateMenuItemCommandValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Menu Id is required");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Authorized user required")
                .Must(userId => Guid.TryParse(userId, out _))
                .WithMessage("Invalid UserId format.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name can't be more than 100 characters.")
                .Matches(@"^[a-zA-Z0-9\s]+$")
                .WithMessage("Name contains invalid characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description can't be more than 512 characters.")
                .Matches(@"^[a-zA-Z0-9\s]+$")
                .WithMessage("Description contains invalid characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("Image URL must be a valid URL.");

        }
    }
}
