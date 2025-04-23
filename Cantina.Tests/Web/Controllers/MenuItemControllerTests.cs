using Cantina.Application.UseCase.Menu.Query.GetMenu;
using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
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
        private readonly Mock<IMenuRepository> _menuRepository;
        public MenuItemControllerTests()
        {
            _menuRepository = new Mock<IMenuRepository>();
            _serviceDescriptors = new ServiceCollection();
            _serviceDescriptors.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetMenuQueryHandler).Assembly));
            _serviceDescriptors.AddLogging();
        }

        [Fact]
        public async Task GetAll_ShouldReturnMenuItems_WhenThereIsMenuItemsAsync()
        {
            var menuItems = new List<MenuItem>
            {
                new() { Id = 1, Name = "Pizza", Price = 10.99m, IsDeleted = false },
                new() { Id = 2, Name = "Pasta", Price = 8.99m, IsDeleted = false }
            };
            _menuRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(menuItems);
            var mediator = CreateMediator(_menuRepository);
            var menuItemController = new MenuItemController(mediator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();
            var okResult = actionResult as OkObjectResult;
            okResult.ShouldNotBeNull();
            var resultMenuItems = okResult.Value as List<MenuItem>;
            resultMenuItems.ShouldNotBeNull();
            resultMenuItems.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAll_ShouldReturnNotFound_WhenNoMenuItemsAsync()
        {
            _menuRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);
            var mediator = CreateMediator(_menuRepository);
            var menuItemController = new MenuItemController(mediator);

            var actionResult = await menuItemController.GetAllAsync();

            actionResult.ShouldNotBeNull();
            var notFoundResult = actionResult as NotFoundObjectResult;
            notFoundResult.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMenuItem_WhenItExistsAsync()
        {
            var dish = new MenuItem { Id = 1, Name = "Pizza", Price = 10.99m, IsDeleted = false };
            _menuRepository.Setup(repo => repo.GetByIdAsync(dish.Id)).ReturnsAsync(dish);
            var mediator = CreateMediator(_menuRepository);
            var menuItemController = new MenuItemController(mediator);

            var actionResult = await menuItemController.GetByIdAsync(dish.Id);

            actionResult.ShouldNotBeNull();
            var okResult = actionResult as OkObjectResult;
            okResult.ShouldNotBeNull();
            var menuItem = okResult.Value as MenuItem;
            menuItem.ShouldNotBeNull();
            menuItem.ShouldBe(dish);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenMenuItemDontExistAsync()
        {
            var itemId = 101;
            _menuRepository.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync((MenuItem)null);
            var mediator = CreateMediator(_menuRepository);
            var menuItemController = new MenuItemController(mediator);

            var actionResult = await menuItemController.GetByIdAsync(itemId);

            actionResult.ShouldNotBeNull();
            var notFoundResult = actionResult as NotFoundObjectResult;
            notFoundResult.ShouldNotBeNull();
        }

        private IMediator CreateMediator(Mock<IMenuRepository> menuItemRepository)
        {
            _serviceDescriptors.AddScoped(_ => menuItemRepository.Object);
            var serviceProvider = _serviceDescriptors.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMediator>();
        }
    }
}
