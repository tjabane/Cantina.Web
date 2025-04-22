using Cantina.Domain.Entities;
using Cantina.Domain.Repositories;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.UseCase.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateReviewCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var menuItem = await _unitOfWork.MenuRepository.GetByIdAsync(request.MenuId);
            if (menuItem is null)
                return Result.Fail(new Error($"Menu item with ID {request.MenuId} not found"));
            var review = new Review
            {
                UserId = request.UserId,
                MenuItemId = request.MenuId,
                Rating = request.Rating,
                Comment = request.Comment
            };
            await _unitOfWork.ReviewRepository.AddReviewAsync(review);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
