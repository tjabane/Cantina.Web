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
using System.Threading.Tasks;

namespace Cantina.Tests.Web.Controllers
{
    public class MenuItemControllerTests
    {
        private readonly ServiceCollection _serviceDescriptors;
        public MenuItemControllerTests()
        {
            _serviceDescriptors = new ServiceCollection();
            _serviceDescriptors.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetMenuQueryHandler).Assembly));
            _serviceDescriptors.AddLogging();
        }

        [Fact]
        public async Task GetAllShouldReturnMenuItemsWhenTheresDataAsync()
        {
            var menuRepo = new Mock<IMenuItemRepository>();
            menuRepo.Setup(repo => repo.GetAllMenuItemsAsync()).ReturnsAsync(GetMenuItems());
            var mediator = CreateMediator(menuRepo);
            var menuItemController = new MenuItemController(mediator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();

            var okResult = actionResult as OkObjectResult;
            okResult.ShouldNotBeNull();

            var menuItems = okResult.Value as List<MenuItemView>;
            menuItems.ShouldNotBeNull();
            menuItems.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllShouldReturnNotFoundWhenNoMenuItemsAsync()
        {
            var menuRepo = new Mock<IMenuItemRepository>();
            menuRepo.Setup(repo => repo.GetAllMenuItemsAsync()).ReturnsAsync([]);
            var mediator = CreateMediator(menuRepo);
            var menuItemController = new MenuItemController(mediator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();
            var notFoundResult = actionResult as NotFoundObjectResult;
            notFoundResult.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllShouldReturnInternalServerErrorWhenExceptionAsync()
        {
            var menuRepo = new Mock<IMenuItemRepository>();
            menuRepo.Setup(repo => repo.GetAllMenuItemsAsync()).ThrowsAsync(new Exception("Test exception"));
            var mediator = CreateMediator(menuRepo);
            var menuItemController = new MenuItemController(mediator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();
            var notFoundResult = actionResult as NotFoundObjectResult;
            notFoundResult.ShouldNotBeNull();
        }

        private IMediator CreateMediator(Mock<IMenuItemRepository> menuItemRepository)
        {
            _serviceDescriptors.AddScoped(_ => menuItemRepository.Object);
            var serviceProvider = _serviceDescriptors.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMediator>();
        }

        private static List<MenuItemView> GetMenuItems()
        {
            return
                [
                    new() {
                        Id = 1,
                        Name = "Taco",
                        Description = "Delicious taco",
                        Price = 2.99m
                    },
                    new() {
                        Id = 2,
                        Name = "Burrito",
                        Description = "Yummy burrito",
                        Price = 5.99m
                    }
                ];
        }
    }
}
