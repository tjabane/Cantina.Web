using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Menu.Commands.DeleteMenuItem
{
    public record DeleteMenuItemCommand(int MenuId, string UserId) : IRequest<Result>;
}
