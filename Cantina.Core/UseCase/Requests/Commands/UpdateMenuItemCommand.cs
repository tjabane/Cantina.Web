using Cantina.Core.Dto;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Requests.Commands
{
    public class UpdateMenuItemCommand(int id, MenuItem menuItem) : IRequest<Result<MenuItem>>
    {
        private readonly int _itemId = id;
        private readonly MenuItem _menuItem = menuItem;
        public MenuItem MenuItem => _menuItem;
        public int Id => _itemId;
    }
}
