using Cantina.Core.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.Validator
{
    public class ReviewValidator: AbstractValidator<Review>
    {
        public ReviewValidator() 
        {
            RuleFor(review => review.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
            RuleFor(review => review.Comment)
                .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.");
            RuleFor(review => review.UserId)
                .NotEmpty().WithMessage("User ID is required.");
            RuleFor(review => review.MenuId)
                .NotEmpty().WithMessage("Menu Item ID is required.");
        }
    }
}
