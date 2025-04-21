using Cantina.Application.Interface;
using Cantina.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Cantina.Application.UseCase.User.Commands.CreateUser
{
    public class CreateUserCommandHandler(IUserManager userManager, ILogger<CreateUserCommandHandler> logger) : IRequestHandler<CreateUserCommand, IdentityResult>
    {
        private readonly IUserManager _userManager = userManager;
        private readonly ILogger<CreateUserCommandHandler> _logger = logger;

        public async Task<IdentityResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };
            var newUserResult =  await _userManager.CreateAsync(user, request.Password);
            if (newUserResult.Succeeded)
            {
                var newUser = await _userManager.FindByEmailAsync(request.Email);
                return await _userManager.AddToRoleAsync(newUser, "Member"); ;
            }
            else
            {
                _logger.LogWarning("User creation failed: {Errors}", string.Join(", ", newUserResult.Errors.Select(e => e.Description)));
                return newUserResult;
            }
        }
    }
}
