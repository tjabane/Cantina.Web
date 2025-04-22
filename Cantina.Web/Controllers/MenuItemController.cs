using Cantina.Application.UseCase.Menu.Commands.AddMenuItem;
using Cantina.Core.Dto;
using Cantina.Core.UseCase.Requests.Commands;
using Cantina.Core.UseCase.Requests.Queries;
using Cantina.Application.UseCase.Menu.Query.GetMenu;
using Cantina.Web.Helpers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cantina.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController(IMediator mediator, ILogger<MenuItemController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<MenuItemController> _logger = logger;

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
            if (menuResponse.IsFailed)
                return NotFound(menuResponse.Errors);
            return Ok(menuResponse.Value);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAsync([FromQuery] string name)
        {
            try
            {
                var menuResponse = await _mediator.Send(new SearchMenuItemQuery(name));
                if (menuResponse.IsFailed)
                    return NotFound(menuResponse.Errors);
                return Ok(menuResponse.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching menu items");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Load shredding, try again later"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] MenuItem menuItem)
        {       
            try {
                await _mediator.Send(new AddMenuItemCommand(menuItem.Name, menuItem.Description, menuItem.Price, menuItem.Image));
                return StatusCode(StatusCodes.Status201Created, menuItem);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while creating menu item");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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
