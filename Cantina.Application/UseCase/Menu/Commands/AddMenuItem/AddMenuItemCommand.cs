using MediatR;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Menu.Commands.AddMenuItem
{
    public record AddMenuItemCommand(string Name, string Description, decimal Price, string ImageUrl) :  IRequest<Result>;

}
