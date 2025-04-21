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
    public class AddMenuItemCommandHandler(IMenuRepository menuRepository) : IRequestHandler<AddMenuItemCommand, Result>
    {
        private readonly IMenuRepository _menuRepository = menuRepository;

        public async Task<Result> Handle(AddMenuItemCommand request, CancellationToken cancellationToken)
        {
            var newMenuItem = new MenuItem
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl
            };

            await _menuRepository.AddAsync(newMenuItem);
            await _menuRepository.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
