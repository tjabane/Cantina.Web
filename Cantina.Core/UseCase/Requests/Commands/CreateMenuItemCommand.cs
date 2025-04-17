using Cantina.Core.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Requests.Commands
{
    public class CreateMenuItemCommand(MenuItem menuItem) : IRequest
    {
        private readonly MenuItem _menuItem = menuItem;
        public MenuItem MenuItem => _menuItem;
    }
}
