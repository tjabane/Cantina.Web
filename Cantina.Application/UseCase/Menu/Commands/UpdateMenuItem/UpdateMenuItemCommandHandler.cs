using Cantina.Domain.Contants;
using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using FluentResults;
using MediatR;


namespace Cantina.Application.UseCase.Menu.Commands.UpdateMenuItem
{
    public class UpdateMenuItemCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateMenuItemCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = await _unitOfWork.MenuRepository.GetByIdAsync(request.Id);
            if (menuItem is null)
                return Result.Fail(new Error($"Menu item with ID {request.Id} not found"));
            menuItem.Name = request.Name;
            menuItem.Description = request.Description;
            menuItem.Price = request.Price;
            menuItem.ImageUrl = request.ImageUrl;
            menuItem.MenuItemTypeId = (int)request.Type;
            var audit = new MenuAudit
            {
                MenuItemId = menuItem.Id,
                UserId = Guid.Parse(request.UserId),
                ActionId = (int)Actions.Update
            };

            _unitOfWork.MenuRepository.UpdateAsync(menuItem);
            await _unitOfWork.MenuAuditRepository.AddAsync(audit);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
