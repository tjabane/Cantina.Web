using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Handlers
{
    public class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, Result<MenuItem>>
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ILogger<CreateMenuItemCommandHandler> _logger;
        
        public CreateMenuItemCommandHandler(IMenuItemRepository menuItemRepository, ILogger<CreateMenuItemCommandHandler> logger)
        {
            _logger = logger;
            _menuItemRepository = menuItemRepository;
        }
        public async Task<Result<MenuItem>> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
        {
            try {
                var menuItem = request.MenuItem;
                await _menuItemRepository.AddMenuItemAsync(menuItem);
                return Result.Ok(menuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu item");
                return Result.Fail(new Error("Error creating menu item"));
            }
        }
    }
}
