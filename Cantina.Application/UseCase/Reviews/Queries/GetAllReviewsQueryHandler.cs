using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using FluentResults;
using MediatR;

namespace Cantina.Application.UseCase.Reviews.Queries
{
    public class GetAllReviewsQueryHandler(IReviewRepository reviewRepository) : IRequestHandler<GetAllReviewsQuery, Result<List<Review>>>
    {
        private readonly IReviewRepository _reviewRepository = reviewRepository;
        public async Task<Result<List<Review>>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
           var reviews = await _reviewRepository.GetAllReviewsAsync();
            if (reviews == null || reviews.Count == 0)
                return Result.Fail(new Error("No reviews found."));
            return Result.Ok(reviews);
        }
    }
}
