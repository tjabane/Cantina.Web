using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Handlers;
using Cantina.Web.Controllers;
using MediatR;
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
        }

        [Fact]
        public async Task GetAllShouldReturnMenuItemsWhenRepositotyIsCalledAsync()
        {
            var menuRepo = new Mock<IMenuItemRepository>();
            menuRepo.Setup(repo => repo.GetAllMenuItemsAsync()).ReturnsAsync([]);
            var mediator = CreateMediator(menuRepo);
            var menuItemController = new MenuItemController(mediator);

            var results = await menuItemController.GetAllAsync();

            results.ShouldNotBeNull();
            menuRepo.Verify(repo => repo.GetAllMenuItemsAsync(), Times.AtLeastOnce);
        }

        private IMediator CreateMediator(Mock<IMenuItemRepository> menuItemRepository)
        {
            _serviceDescriptors.AddScoped(_ => menuItemRepository.Object);
            var serviceProvider = _serviceDescriptors.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMediator>();
        }

    }
}
