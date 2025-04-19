using Cantina.Core.Dto;
using Cantina.Core.Validator;
using FluentValidation;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Tests.Core.Validator
{
    public class ReviewValidatorTests
    {
        private readonly IValidator<Review> _validator;
        public ReviewValidatorTests()
        {
            _validator = new ReviewValidator();
        }

        [Fact]
        public void Validate_ShouldReturnValid_WhenMenuItemIsValid()
        {
            var review = new Review
            {
                MenuId = 1,
                UserId = 1,
                Rating = 5,
                Comment = "Great food!"
            };
            var result = _validator.Validate(review);
            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_ShouldReturnInvalid_WhenRatingMoreThan5()
        {
            var review = new Review
            {
                MenuId = 1,
                UserId = 1,
                Rating = 6,
                Comment = "Great food!"
            };

            var result = _validator.Validate(review);

            result.IsValid.ShouldBeFalse();
            result.Errors[0].ErrorMessage.ShouldBe("Rating must be between 1 and 5.");
        }

        [Fact]
        public void Validate_ShouldReturnInvalid_WhenRatingLessThan1()
        {
            var review = new Review
            {
                MenuId = 1,
                UserId = 1,
                Rating = 0,
                Comment = "Bad food!"
            };

            var result = _validator.Validate(review);

            result.IsValid.ShouldBeFalse();
            result.Errors[0].ErrorMessage.ShouldBe("Rating must be between 1 and 5.");
        }

        [Fact]
        public void Validate_ShouldReturnInvalid_WhenMenuIdIsZero()
        {
            var review = new Review
            {
                MenuId = 0,
                UserId = 1,
                Rating = 3,
                Comment = "Bad food!"
            };

            var result = _validator.Validate(review);

            result.IsValid.ShouldBeFalse();
            result.Errors[0].ErrorMessage.ShouldBe("Menu Item ID is required.");
        }

        [Fact]
        public void Validate_ShouldReturnInvalid_WhenUserIdIsZero()
        {
            var review = new Review
            {
                MenuId = 1,
                UserId = 0,
                Rating = 3,
                Comment = "Bad food!"
            };

            var result = _validator.Validate(review);

            result.IsValid.ShouldBeFalse();
            result.Errors[0].ErrorMessage.ShouldBe("User ID is required.");
        }

        [Fact]
        public void Validate_ShouldReturnInvalid_WhenCommentIsMoreThan500Characters()
        {
            var review = new Review
            {
                MenuId = 1,
                UserId = 1,
                Rating = 3,
                Comment = new string('A', 501)
            };

            var result = _validator.Validate(review);

            result.IsValid.ShouldBeFalse();
            result.Errors[0].ErrorMessage.ShouldBe("Comment must not exceed 500 characters.");
        }
    }
}
