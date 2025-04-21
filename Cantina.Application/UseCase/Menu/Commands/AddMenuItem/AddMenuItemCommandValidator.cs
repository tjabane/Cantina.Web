using FluentValidation;


namespace Cantina.Application.UseCase.Menu.Commands.AddMenuItem
{
    public class AddMenuItemCommandValidator : AbstractValidator<AddMenuItemCommand>
    {
        public AddMenuItemCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name can't be more than 100 characters.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description can't be more than 512 characters.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("Image URL must be a valid URL.");
        }
    }
}
