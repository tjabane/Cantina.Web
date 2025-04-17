using Cantina.Core.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.Validator
{
    public class MenuItemValidator : AbstractValidator<MenuItem>
    {
        public MenuItemValidator() 
        {
            RuleFor(item => item.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(item => item.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(item => item.Price).NotEmpty().WithMessage("Price is required.")
                                       .GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(item => item.Image).NotEmpty().WithMessage("Image is required.");
        }
    }
}
