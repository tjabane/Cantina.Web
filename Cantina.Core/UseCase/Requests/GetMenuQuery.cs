using Cantina.Core.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Requests
{
    public class GetMenuQuery: IRequest<List<MenuItem>>
    {
        public GetMenuQuery() {}
    }
}
