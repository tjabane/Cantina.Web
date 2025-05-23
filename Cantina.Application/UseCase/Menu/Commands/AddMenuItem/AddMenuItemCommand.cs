﻿using MediatR;
using FluentResults;
using Cantina.Domain.Entities;

namespace Cantina.Application.UseCase.Menu.Commands.AddMenuItem
{
    public class AddMenuItemCommand : IRequest<Result<MenuItem>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public Domain.Contants.MenuItemType Type { get; set; } = Domain.Contants.MenuItemType.Food;
    }
}
