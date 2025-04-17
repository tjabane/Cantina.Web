using Cantina.Core.Dto;
using Cantina.Core.Validator;
using FluentValidation;
using Shouldly;


namespace Cantina.Tests.Core.Validator
{
    public class MenuItemValidatorTests
    {
        private readonly IValidator<MenuItem> _validator;
        public MenuItemValidatorTests()
        {
            _validator = new MenuItemValidator();
        }

        [Fact]
        public void Validate_ShouldReturnValid_WhenMenuItemIsValid()
        {
            var menuItem = new MenuItem
            {
                Name = "Pizza",
                Description = "Delicious cheese pizza",
                Price = 9.99m,
                Image = "Main Course"
            };
            
            
            var result = _validator.Validate(menuItem);
            
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Validate_ShouldReturnInValid_WhenMenuItemNameIsNull(string name)
        {
            var menuItem = GetMenuItem(name);

            var result = _validator.Validate(menuItem);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldNotBeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Validate_ShouldReturnInValid_WhenDescriptionIsNull(string description)
        {
            var menuItem = GetMenuItem(description: description);

            var result = _validator.Validate(menuItem);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldNotBeEmpty();
        }

        [Fact]
        public void Validate_ShouldReturnInValid_WhenPriceLessThanZero()
        {
            var menuItem = GetMenuItem(price: -9.00m);

            var result = _validator.Validate(menuItem);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldNotBeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Validate_ShouldReturnInValid_WhenImageIsNull(string image)
        {
            var menuItem = GetMenuItem(image: image);

            var result = _validator.Validate(menuItem);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldNotBeEmpty();
        }



        private static MenuItem GetMenuItem(string name="Pizza", string description = "Delicious", decimal price = 9.99m, string image="image.png")
        {
            return new()
            {
                Name = name,
                Description = description,
                Price = price,
                Image = image
            };
        }
    }
}
