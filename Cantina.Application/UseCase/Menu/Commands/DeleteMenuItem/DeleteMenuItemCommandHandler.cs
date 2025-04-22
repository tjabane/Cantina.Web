using Cantina.Domain.Contants;
using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Menu.Commands.DeleteMenuItem
{
    public class DeleteMenuItemCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMenuItemCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = await _unitOfWork.MenuRepository.GetByIdAsync(request.MenuId);
            if (menuItem is null)
                return Result.Fail(new Error($"Menu item with ID {request.MenuId} not found"));
            menuItem.IsDeleted = true;
            var audit = new MenuAudit
            {
                MenuItemId = menuItem.Id,
                UserId = Guid.Parse(request.UserId),
                ActionId = (int)Actions.Delete
            };
            _unitOfWork.MenuRepository.UpdateAsync(menuItem);
            await _unitOfWork.MenuAuditRepository.AddAsync(audit);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
