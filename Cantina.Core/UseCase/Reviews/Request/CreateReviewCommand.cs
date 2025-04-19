using Cantina.Core.Dto;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Reviews.Request
{
    public class CreateReviewCommand(Review review) : IRequest<Result>
    {
        private readonly Review _review = review;
        public Review Review => _review;
    }
}
