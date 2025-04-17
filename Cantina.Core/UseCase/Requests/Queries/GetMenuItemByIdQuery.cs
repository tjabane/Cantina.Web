using Cantina.Core.Dto;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Requests.Queries
{
    public class GetMenuItemByIdQuery(int id) : IRequest<Result<MenuItem>>
    {
        private readonly int _itemId = id;

        public int ItemId => _itemId;
    }
}
