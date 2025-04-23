using FluentValidation;


namespace Cantina.Application.UseCase.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandValidator: AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Authorized user required")
                .Must(userId => Guid.TryParse(userId, out _))
                .WithMessage("Invalid UserId format.");
            RuleFor(x => x.MenuId)
                .NotEmpty()
                .WithMessage("Menu ID is required.");
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.");
            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("Comment is required.")
                .MaximumLength(500)
                .WithMessage("Comment cannot exceed 500 characters.")
                .Matches(@"^[a-zA-Z0-9\s]+$")
                .WithMessage("Comment contains invalid characters.");
        }
    }
}
