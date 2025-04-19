using Cantina.Core.Data.Entities;
using Cantina.Core.Dto;
using Cantina.Core.Interface;
using Cantina.Core.UseCase.Requests.Queries;
using Cantina.Core.UseCase.Reviews.Request;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Reviews.Handlers
{
    public class GetAllReviewsQueryHandler(IReviewQueryRepository reviewRepository) : IRequestHandler<GetAllReviewsQuery, Result<List<ReviewView>>>
    {
        private readonly IReviewQueryRepository _reviewRepository = reviewRepository;
        public async Task<Result<List<ReviewView>>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewRepository.GetAllAsync();
            if(reviews == null || reviews.Count == 0)
                return Result.Fail("No reviews found");
            return Result.Ok(reviews);
        }
    }
}
