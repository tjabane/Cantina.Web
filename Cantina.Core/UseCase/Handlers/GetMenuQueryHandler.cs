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
    public class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, Result<List<MenuItem>>>
    {
        private readonly ILogger<GetMenuQueryHandler> _logger;
        private readonly IMenuItemRepository _menuItemRepository;
        public GetMenuQueryHandler(IMenuItemRepository menuItemRepository, ILogger<GetMenuQueryHandler> logger)
        {
            _logger = logger;
            _menuItemRepository = menuItemRepository;
        }
        public async Task<Result<List<MenuItem>>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            try {
                var response = await _menuItemRepository.GetAllMenuItemsAsync();
                if (response == null || response.Count == 0)
                {
                    _logger.LogWarning("No menu items found.");
                    return Result.Fail("Menu items not found.");
                }
                return Result.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting menu items.");
                return Result.Fail("An error occurred while processing your request.");
            }
        }
    }
}
