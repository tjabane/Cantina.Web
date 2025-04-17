using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Handlers;
using Cantina.Core.Validator;
using Cantina.Web.Controllers;
using Cantina.Web.Helpers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cantina.Tests.Web.Controllers
{
    public class MenuItemControllerTests
    {
        private readonly IValidator<MenuItem> _validator;
        private readonly ServiceCollection _serviceDescriptors;
        private readonly Mock<IMenuItemRepository> _menuItemRepository;

        
        public MenuItemControllerTests()
        {
            _validator = new MenuItemValidator();
            _menuItemRepository = new Mock<IMenuItemRepository>();
            _serviceDescriptors = new ServiceCollection();
            _serviceDescriptors.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetMenuQueryHandler).Assembly));
            _serviceDescriptors.AddLogging();
        }

        [Fact]
        public async Task GetAll_ShouldReturnMenuItems_WhenTheresDataAsync()
        {
            _menuItemRepository.Setup(repo => repo.GetAllMenuItemsAsync()).ReturnsAsync([GetMenuItem()]);
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator, _validator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();

            var okResult = actionResult as OkObjectResult;
            okResult.ShouldNotBeNull();
            var menuItems = okResult.Value as List<MenuItem>;
            menuItems.ShouldNotBeNull();
            menuItems.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyMenu_WhenNoDataAsync()
        {
            _menuItemRepository.Setup(repo => repo.GetAllMenuItemsAsync()).ReturnsAsync([]);
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator, _validator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();
            var notFoundResult = actionResult as OkObjectResult;
            notFoundResult.ShouldNotBeNull();
            var menuItems = notFoundResult.Value as List<MenuItem>;
            menuItems.ShouldBeEmpty();
        }

        [Fact]
        public async Task GetAll_ShouldReturn500InternalServerError_WhenExceptionAsync()
        {
            _menuItemRepository.Setup(repo => repo.GetAllMenuItemsAsync()).ThrowsAsync(new Exception("Test exception"));
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator, _validator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();
            var internalServerErrorResult = actionResult as StatusCodeResult;
            internalServerErrorResult.ShouldNotBeNull();
            internalServerErrorResult.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GetMenuItemById_ShouldReturnMenuItem_WhenItExistsAsync()
        {
            var dish = GetMenuItem();
            _menuItemRepository.Setup(repo => repo.GetItemByIdAsync(dish.Id)).ReturnsAsync(dish);
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator, _validator);

            var actionResult = await menuItemController.GetById(dish.Id);

            actionResult.ShouldNotBeNull();
            var okResult = actionResult as OkObjectResult;
            okResult.ShouldNotBeNull();
            var menuItem = okResult.Value as MenuItem;
            menuItem.ShouldNotBeNull();
            menuItem.ShouldBe(dish);
        }



        [Fact]
        public async Task CreateMenuItem_ShouldReturnCreated_WhenRequestIsValidAsync()
        {
            _menuItemRepository.Setup(repo => repo.AddMenuItemAsync(It.IsAny<MenuItem>())).Verifiable();
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator, _validator);
            var menuItem = GetMenuItem();

            var actionResult = await menuItemController.CreateAsync(menuItem);

            actionResult.ShouldNotBeNull();
            var createdResult = actionResult as CreatedAtActionResult;
            createdResult.ShouldNotBeNull();
            _menuItemRepository.Verify(repo => repo.AddMenuItemAsync(menuItem), Times.AtMostOnce());
        }

        [Fact]
        public async Task CreateMenuItem_ShouldReturnBadRequest_WhenRequestBodyIsInvalidAsync()
        {
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator, _validator);
            var menuItem = new MenuItem() { };

            var actionResult = await menuItemController.CreateAsync(menuItem);

            actionResult.ShouldNotBeNull();
            var badRequestResult = actionResult as BadRequestObjectResult;
            badRequestResult.ShouldNotBeNull();
            var response = badRequestResult.Value as Response;
            response.ShouldNotBeNull();
            response.Message.ShouldBe("Invalid Request Parameters");
            response.Desciption.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateMenuItem_ShouldReturn500InternalServerError_WhenExceptionOccursAsync()
        {
            _menuItemRepository.Setup(repo => repo.AddMenuItemAsync(It.IsAny<MenuItem>())).ThrowsAsync(new Exception("Error"));
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator, _validator);
            var menuItem = GetMenuItem();

            var actionResult = await menuItemController.CreateAsync(menuItem);

            actionResult.ShouldNotBeNull();
            var internalServerErrorResult = actionResult as StatusCodeResult;
            internalServerErrorResult.ShouldNotBeNull();
            internalServerErrorResult.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        }


        private IMediator CreateMediator(Mock<IMenuItemRepository> menuItemRepository)
        {
            _serviceDescriptors.AddScoped(_ => menuItemRepository.Object);
            var serviceProvider = _serviceDescriptors.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMediator>();
        }

        private static MenuItem GetMenuItem()
        {
            return new()
            {
                Id = 1,
                Name = "Taco",
                Description = "Delicious taco",
                Price = 2.99m,
                Image = "taco.jpg",
                AverageRating = 4.5
            };
        }
    }
}
