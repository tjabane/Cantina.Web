using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests;
using Cantina.Core.UseCase.Requests.Commands;
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
    public class CreateMenuItemCommandHandler(IMenuItemRepository menuItemRepository) : IRequestHandler<CreateMenuItemCommand>
    {
        private readonly IMenuItemRepository _menuItemRepository = menuItemRepository;

        public async Task Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = request.MenuItem;
            await _menuItemRepository.AddAsync(menuItem);
        }
    }
}
