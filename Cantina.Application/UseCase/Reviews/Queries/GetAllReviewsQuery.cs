using Cantina.Domain.Entities;
using FluentResults;
using MediatR;


namespace Cantina.Application.UseCase.Reviews.Queries
{
    public class GetAllReviewsQuery: IRequest<Result<List<Review>>>
    {
    }
}
