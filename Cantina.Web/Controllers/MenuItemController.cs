using Cantina.Core.Dto;
using Cantina.Core.UseCase.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cantina.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var menuResponse = await _mediator.Send(new GetMenuQuery());
            return Ok(menuResponse);
        }

    }
}
