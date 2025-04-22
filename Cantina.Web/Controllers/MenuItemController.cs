using Cantina.Application.UseCase.Menu.Commands.AddMenuItem;
using Cantina.Core.Dto;
using Cantina.Core.UseCase.Requests.Commands;
using Cantina.Application.UseCase.Menu.Query.GetMenu;
using Cantina.Web.Helpers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cantina.Application.UseCase.Menu.Query.GetMenuItemById;
using Cantina.Application.UseCase.Menu.Query.SearchMenu;
using Cantina.Web.Dto;
using System.IdentityModel.Tokens.Jwt;

namespace Cantina.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController(IMediator mediator) : ControllerBase
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
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var addItemCmd = new AddMenuItemCommand()
            {
                UserId = userId,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                ImageUrl = menuItem.ImageUrl,
                Type = menuItem.Type,
            };
            var newItem = await _mediator.Send(addItemCmd);
            return StatusCode(StatusCodes.Status201Created, newItem.Value);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] MenuItem menuItem)
        {
            var updateResult = await _mediator.Send(new UpdateMenuItemCommand(id, menuItem));
            if(updateResult.IsFailed)
                return NotFound(updateResult.Errors);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleteResult = await _mediator.Send(new DeleteMenuItemCommand(id));
            if (deleteResult.IsFailed)
                return NotFound(deleteResult.Errors);
            return NoContent();
        }
    }
}
