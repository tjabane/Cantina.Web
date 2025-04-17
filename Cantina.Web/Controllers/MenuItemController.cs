using Cantina.Core.Dto;
using Cantina.Core.UseCase.Requests;
using Cantina.Core.UseCase.Requests.Commands;
using Cantina.Core.UseCase.Requests.Queries;
using Cantina.Web.Helpers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cantina.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController(IMediator mediator, IValidator<MenuItem> validator, ILogger<MenuItemController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IValidator<MenuItem> _validator = validator;
        private readonly ILogger<MenuItemController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var menuResponse = await _mediator.Send(new GetMenuQuery());
                if (menuResponse.IsFailed)
                    return NotFound(menuResponse.Errors);
                return Ok(menuResponse.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading menu items");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Load shredding, try again later"));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var menuResponse = await _mediator.Send(new GetMenuItemByIdQuery(id));
                if (menuResponse.IsFailed)
                    return NotFound(menuResponse.Errors);
                return Ok(menuResponse.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading menu item");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Load shredding, try again later"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] MenuItem menuItem)
        {
            try {
                var validationResult = await _validator.ValidateAsync(menuItem);
                if (!validationResult.IsValid)
                    return BadRequest(new Response(validationResult.Errors));
                await _mediator.Send(new CreateMenuItemCommand(menuItem));
                return CreatedAtAction(nameof(CreateAsync), menuItem);
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while creating menu item");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Load shredding, try again later"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] MenuItem menuItem)
        {
            var validationResult = await _validator.ValidateAsync(menuItem);
            if (!validationResult.IsValid)
                return BadRequest(new Response(validationResult.Errors));
            var updateResult = await _mediator.Send(new UpdateMenuItemCommand(id, menuItem));
            if(updateResult.IsFailed)
                return NotFound(updateResult.Errors);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try {
                var deleteResult = await _mediator.Send(new DeleteMenuItemCommand(id));
                if (deleteResult.IsFailed)
                    return NotFound(deleteResult.Errors);
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting menu item");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Load shredding, try again later"));
            }
        }
    }
}
