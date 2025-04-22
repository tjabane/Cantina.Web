using Cantina.Application.UseCase.Menu.Query.GetMenu;
using Cantina.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cantina.Application.UseCase.Menu.Query.GetMenuItemById;
using Cantina.Application.UseCase.Menu.Query.SearchMenu;
using Cantina.Web.Dto;
using Cantina.Application.UseCase.Menu.Commands.DeleteMenuItem;
using Cantina.Web.Abstration;

namespace Cantina.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController(IMediator mediator) : CantinaController
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var menuResponse = await _mediator.Send(new GetMenuQuery());
            return menuResponse.IsFailed ? NotFound(menuResponse.Errors) : Ok(menuResponse.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var menuResponse = await _mediator.Send(new GetMenuItemByIdQuery(id));
            return menuResponse.IsFailed? NotFound(menuResponse.Errors): Ok(menuResponse.Value);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAsync([FromQuery] string search)
        {
            var menuResponse = await _mediator.Send(new SearchMenuItemQuery(search));
            return menuResponse.IsFailed? NotFound(menuResponse.Errors) : Ok(menuResponse.Value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] MenuItemDto menuItem)
        {
            var addItemCmd = CommandFactory.CreateAddMenuItemCommand(menuItem, CurrentUserId);
            await _mediator.Send(addItemCmd);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] MenuItemDto menuItem, int id)
        {
            var updateCmd = CommandFactory.CreateUpdateMenuItemCommand(menuItem, id, CurrentUserId);
            var updateResult = await _mediator.Send(updateCmd);
            return updateResult.IsFailed? NotFound(updateResult.Errors): NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleteResult = await _mediator.Send(new DeleteMenuItemCommand(id, CurrentUserId));
            return deleteResult.IsFailed? NotFound(deleteResult.Errors) : NoContent();
        }
    }
}
