using Cantina.Application.Interface;
using Cantina.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cantina.Application.UseCase.User.Commands.CreateUser
{
    public class CreateUserCommandHandler(IUserManager userManager) : IRequestHandler<CreateUserCommand, IdentityResult>
    {
        private readonly IUserManager _userManager = userManager;

        public async Task<IdentityResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };
            return await _userManager.CreateAsync(user, request.Password);
        }
    }
}
