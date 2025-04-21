using Cantina.Application.Interface;
using Cantina.Domain.Entities;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;


namespace Cantina.Application.UseCase.User.Queries.Login
{
    internal class LoginQueryHandler(IUserManager userManager, ITokenProvider tokenProvider) : IRequestHandler<LoginQuery, Result<string>>
    {
        private readonly IUserManager _userManager = userManager;
        private readonly ITokenProvider _tokenProvider = tokenProvider;

        public async Task<Result<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await  _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Fail("Invalid username or password.");
            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValidPassword)
                return Result.Fail("Invalid username or password.");
            var userRoles = await _userManager.GetRolesAsync(user);
           return _tokenProvider.GenerateToken(user, userRoles.ToList());
        }
    }
}