using Cantina.Core.Interface;
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
    public class CreateReviewCommandHandler(IReviewCommandRepository reviewCommandRepository, IMenuQueryRepository menuQueryRepository, IUserRepository userRepository) : IRequestHandler<CreateReviewCommand, Result>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMenuQueryRepository _menuQueryRepository = menuQueryRepository;
        private readonly IReviewCommandRepository _reviewCommandRepository = reviewCommandRepository;

        public async Task<Result> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var menu = await _menuQueryRepository.GetByIdAsync(request.Review.MenuId);
            if (menu is null)
                return Result.Fail("Provided menu doesnt exist");
            var user = await _userRepository.GetById(request.Review.UserId);
            if(user is null)
                return Result.Fail("Provided user doesnt exist");
            await _reviewCommandRepository.AddAsync(request.Review);
            return Result.Ok();
        }
    }
}
