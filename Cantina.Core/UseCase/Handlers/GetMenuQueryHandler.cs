using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Handlers
{
    public class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, List<MenuItem>>
    {
        private readonly IMenuItemRepository _menuItemRepository;
        public GetMenuQueryHandler(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }
        public async Task<List<MenuItem>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            return await _menuItemRepository.GetAllMenuItemsAsync();
        }
    }
}
