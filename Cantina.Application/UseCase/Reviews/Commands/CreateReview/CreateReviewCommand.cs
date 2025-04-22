using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Reviews.Commands.CreateReview
{
    public record CreateReviewCommand(string UserId, int MenuId, int Rating, string Comment) : IRequest<Result>;
}
