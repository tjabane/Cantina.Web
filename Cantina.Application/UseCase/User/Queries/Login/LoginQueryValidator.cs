using FluentValidation;

namespace Cantina.Application.UseCase.User.Queries.Login
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(cred => cred.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(cred => cred.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
