using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.UseCase.User.Commands.CreateUser
{
    public class CreateUserCommand(string fullName, string email, string password) : IRequest<IdentityResult>
    {
        public string FullName { get; set; } = fullName;
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
    }
}
