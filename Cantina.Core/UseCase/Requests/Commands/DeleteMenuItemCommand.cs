using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Requests.Commands
{
    public class DeleteMenuItemCommand(int id) : IRequest<Result>
    {
        public int Id { get; } = id;
    }
}
