using Cantina.Core.Dto;
using Cantina.Core.UseCase.Requests;
using Cantina.Web.Helpers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cantina.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController(IMediator mediator, IValidator<MenuItem> validator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IValidator<MenuItem> _validator = validator;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var menuResponse = await _mediator.Send(new GetMenuQuery());
            if (menuResponse.IsFailed)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(menuResponse.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] MenuItem menuItem)
        {
            var validationResult = await _validator.ValidateAsync(menuItem);
            if (!validationResult.IsValid) 
                return BadRequest(new Response(validationResult.Errors));

            var createResult = await _mediator.Send(new CreateMenuItemCommand(menuItem));
            return createResult.IsFailed? StatusCode(StatusCodes.Status500InternalServerError)
                                        : CreatedAtAction(nameof(CreateAsync), menuItem);
        }
    }
}
