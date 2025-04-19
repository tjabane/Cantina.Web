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
    public class CreateReviewCommand(ReviewDto review) : IRequest
    {
        private readonly ReviewDto _review = review;
        public ReviewDto Review => _review;
    }
}
