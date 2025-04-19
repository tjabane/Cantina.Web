using Cantina.Core.Interface;
using Cantina.Core.UseCase.Reviews.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.Reviews.Handlers
{
    public class CreateReviewCommandHandler(IReviewCommandRepository reviewCommandRepository) : IRequestHandler<CreateReviewCommand>
    {
        private readonly IReviewCommandRepository _reviewCommandRepository = reviewCommandRepository;
        public async Task Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            await _reviewCommandRepository.AddAsync(request.Review);
        }
    }
}
