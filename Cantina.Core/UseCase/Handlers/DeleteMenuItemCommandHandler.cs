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
    public class DeleteMenuItemCommandHandler(IMenuCommandRepository menuCmdRepository, IMenuQueryRepository menuQueryRepository) : IRequestHandler<DeleteMenuItemCommand, Result>
    {
        private readonly IMenuCommandRepository _menuCmdRepository = menuCmdRepository;
        private readonly IMenuQueryRepository _menuQueryRepository = menuQueryRepository;

        public async Task<Result> Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = await _menuQueryRepository.GetByIdAsync(request.Id);
            if (menuItem is null)
                return Result.Fail(new Error($"Menu item with id {request.Id} not found"));
            await _menuCmdRepository.DeleteAsync(request.Id);
            return Result.Ok();
        }
    }
}
