using FluentValidation;


namespace Cantina.Application.UseCase.Menu.Commands.DeleteMenuItem
{
    public class DeleteMenuItemCommandValidator : AbstractValidator<DeleteMenuItemCommand>
    {
        public DeleteMenuItemCommandValidator() 
        {
            RuleFor(x => x.MenuId)
                .NotEmpty()
                .WithMessage("Menu ID cannot be empty.");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Authorized user required")
                .Must(userId => Guid.TryParse(userId, out _))
                .WithMessage("Invalid UserId format.");
        }
    }
}
