using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests;
using Cantina.Core.UseCase.Requests.Commands;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Handlers
{
    public class DeleteMenuItemCommandHandler(IMenuItemRepository menuItemRepository) : IRequestHandler<DeleteMenuItemCommand, Result>
    {
        private readonly IMenuItemRepository _menuItemRepository = menuItemRepository;

        public async Task<Result> Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(request.Id);
            if (menuItem is null)
                return Result.Fail(new Error($"Menu item with id {request.Id} not found"));
            await _menuItemRepository.DeleteAsync(request.Id);
            return Result.Ok();
        }
    }
}
