using Cantina.Application.UseCase.Reviews.Commands.CreateReview;
using Cantina.Domain.Entities;
using FluentValidation;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Tests.Application.UseCaseValidation
{
    public class CreateReviewCommandValidatorTests
    {
        private readonly IValidator<CreateReviewCommand> _validator;
        public CreateReviewCommandValidatorTests()
        {
            _validator = new CreateReviewCommandValidator();
        }

        [Fact]
        public void Validate_ShouldReturnValid_WhenCommandIsValid()
        {
            var command = new CreateReviewCommand(Guid.NewGuid().ToString(),1,1, "Great food");

            var result = _validator.Validate(command);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_ShouldReturnInvalid_WhenUserIdIsEmpty()
        {
            var command = new CreateReviewCommand(string.Empty, 1, 1, "Great food");
            var result = _validator.Validate(command);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == "Authorized user required");
        }

        [Fact]
        public void Validate_ShouldReturnInvalid_WhenUserIdIsInvalid()
        {
            var command = new CreateReviewCommand("invalid-guid", 1, 1, "Great food");
            var result = _validator.Validate(command);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == "Invalid UserId format.");
        }

        [Fact]
        public void Validate_ShouldReturnInvalid_WhenMenuIdIsEmpty()
        {
            var command = new CreateReviewCommand(Guid.NewGuid().ToString(), 0, 1, "Great food");
            var result = _validator.Validate(command);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == "Menu ID is required.");
        }

        [Fact]
        public void Validate_ShouldReturnInvalid_WhenRatingIsOutOfRange()
        {
            var command = new CreateReviewCommand(Guid.NewGuid().ToString(), 1, 6, "Great food");
            var result = _validator.Validate(command);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == "Rating must be between 1 and 5.");
        }
        [Fact]
        public void Validate_ShouldReturnInvalid_WhenCommentIsEmpty()
        {
            var command = new CreateReviewCommand(Guid.NewGuid().ToString(), 1, 1, string.Empty);
            var result = _validator.Validate(command);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == "Comment is required.");
        }
        [Fact]
        public void Validate_ShouldReturnInvalid_WhenCommentExceedsMaxLength()
        {
            var command = new CreateReviewCommand(Guid.NewGuid().ToString(), 1, 1, new string('a', 501));
            var result = _validator.Validate(command);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == "Comment cannot exceed 500 characters.");
        }
        [Fact]
        public void Validate_ShouldReturnInvalid_WhenCommentContainsInvalidCharacters()
        {
            var command = new CreateReviewCommand(Guid.NewGuid().ToString(), 1, 1, "Great <script>alert(sadfsdfsdf)</script>");
            var result = _validator.Validate(command);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == "Comment contains invalid characters.");
        }
    }
}
