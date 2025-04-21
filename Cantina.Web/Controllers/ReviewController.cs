using Cantina.Core.Dto;
using Cantina.Core.UseCase.Requests.Commands;
using Cantina.Core.UseCase.Reviews.Request;
using Cantina.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Review = Cantina.Core.Dto.Review;

namespace Cantina.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync()
        {
            var reviewsResponse = await _mediator.Send(new GetAllReviewsQuery());
            if (reviewsResponse.IsFailed)
                return NotFound(reviewsResponse.Errors);
            return Ok(reviewsResponse.Value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Member")]
        public async Task<IActionResult> CreateAsync([FromBody] Review review)
        {
            await _mediator.Send(new CreateReviewCommand(review));
            return StatusCode(StatusCodes.Status201Created, review);
        }
    }
}
