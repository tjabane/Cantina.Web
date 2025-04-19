using Cantina.Core.Data.Entities;
using Cantina.Core.Dto;
using Cantina.Core.UseCase.Requests.Commands;
using Cantina.Core.UseCase.Reviews.Request;
using Cantina.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cantina.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IMediator mediator, ILogger<ReviewController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<ReviewController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var reviewsResponse = await _mediator.Send(new GetAllReviewsQuery());
                if (reviewsResponse.IsFailed)
                    return NotFound(reviewsResponse.Errors);
                return Ok(reviewsResponse.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading reviews");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Something bad happened"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ReviewDto review)
        {
            try
            {
                await _mediator.Send(new CreateReviewCommand(review));
                return StatusCode(StatusCodes.Status201Created, review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a review");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Keyboad not found, press F1 to restart"));
            }
        }
    }
}
