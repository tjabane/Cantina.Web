using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Menu.Commands.UpdateMenuItem
{
    public class UpdateMenuItemCommand: IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public Domain.Contants.MenuItemType Type { get; set; } = Domain.Contants.MenuItemType.Food;
    }
}
