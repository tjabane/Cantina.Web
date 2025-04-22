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

namespace Cantina.Application.UseCase.Menu.Commands.AddMenuItem
{
    public class AddMenuItemCommandHandler(IMenuRepository menuRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddMenuItemCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMenuRepository _menuRepository = menuRepository;

        public async Task<Result> Handle(AddMenuItemCommand request, CancellationToken cancellationToken)
        {
            var newMenuItem = new MenuItem
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                MenuItemTypeId = (int)request.Type
            };
            var audit = new MenuAudit()
            {
                UserId = request.UserId,
                MenuItemId = newMenuItem.Id,
                ActionId = (int)Actions.Create

            };

            await _unitOfWork.MenuRepository.AddAsync(newMenuItem);
            await _unitOfWork.MenuAuditRepository.AddAsync(audit);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
