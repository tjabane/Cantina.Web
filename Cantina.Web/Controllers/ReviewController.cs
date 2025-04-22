using Cantina.Application.UseCase.Reviews.Commands.CreateReview;
using Cantina.Application.UseCase.Reviews.Queries;
using Cantina.Web.Abstration;
using Cantina.Web.Dto;
using Cantina.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cantina.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IMediator mediator) : CantinaController
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync()
        {
            var reviewsResults = await _mediator.Send(new GetAllReviewsQuery());
            return reviewsResults.IsFailed ? NotFound(reviewsResults.Errors) : Ok(reviewsResults.Value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Member")]
        public async Task<IActionResult> CreateAsync([FromBody] ReviewDto review)
        {
            var results = await _mediator.Send(new CreateReviewCommand(CurrentUserId, review.MenuId, review.Rating, review.Comment));
            return results.IsFailed ? NotFound(results.Errors) : StatusCode(StatusCodes.Status201Created, review);
        }
    }
}
