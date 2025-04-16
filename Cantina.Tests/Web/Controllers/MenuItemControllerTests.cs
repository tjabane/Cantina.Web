using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Handlers;
using Cantina.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cantina.Tests.Web.Controllers
{
    public class MenuItemControllerTests
    {
        private readonly Mock<IMenuItemRepository> _menuItemRepository;
        private readonly ServiceCollection _serviceDescriptors;
        public MenuItemControllerTests()
        {
            _menuItemRepository = new Mock<IMenuItemRepository>();
            _serviceDescriptors = new ServiceCollection();
            _serviceDescriptors.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetMenuQueryHandler).Assembly));
            _serviceDescriptors.AddLogging();
        }

        [Fact]
        public async Task GetAllShouldReturnMenuItemsWhenTheresDataAsync()
        {

            _menuItemRepository.Setup(repo => repo.GetAllMenuItemsAsync()).ReturnsAsync([GetMenuItem()]);
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();

            var okResult = actionResult as OkObjectResult;
            okResult.ShouldNotBeNull();
            var menuItems = okResult.Value as List<MenuItem>;
            menuItems.ShouldNotBeNull();
            menuItems.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAllShouldReturnNotFoundWhenNoMenuItemsAsync()
        {
            _menuItemRepository.Setup(repo => repo.GetAllMenuItemsAsync()).ReturnsAsync([]);
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();
            var notFoundResult = actionResult as NotFoundObjectResult;
            notFoundResult.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllShouldReturnInternalServerErrorWhenExceptionAsync()
        {
            _menuItemRepository.Setup(repo => repo.GetAllMenuItemsAsync()).ThrowsAsync(new Exception("Test exception"));
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();
            var notFoundResult = actionResult as NotFoundObjectResult;
            notFoundResult.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateMenuItemShouldReturnCreatedWhenRequestIsValidAsync()
        {
            _menuItemRepository.Setup(repo => repo.AddMenuItemAsync(It.IsAny<MenuItem>())).Verifiable();
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator);
            var menuItem = GetMenuItem();

            var actionResult = await menuItemController.CreateAsync(menuItem);

            actionResult.ShouldNotBeNull();
            var createdResult = actionResult as CreatedAtActionResult;
            createdResult.ShouldNotBeNull();
            _menuItemRepository.Verify(repo => repo.AddMenuItemAsync(menuItem), Times.AtMostOnce());
        }

        [Fact]
        public async Task CreateMenuItemShouldReturnBadRequestWhenRequestBodyIsInvalidAsync()
        {
            _menuItemRepository.Setup(repo => repo.AddMenuItemAsync(It.IsAny<MenuItem>())).Verifiable();
            var mediator = CreateMediator(_menuItemRepository);
            var menuItemController = new MenuItemController(mediator);
            var menuItem = new MenuItem() { };

            var actionResult = await menuItemController.CreateAsync(menuItem);

            actionResult.ShouldNotBeNull();
            var badRequestResult = actionResult as BadRequestResult;
            badRequestResult.ShouldNotBeNull();
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
