using MediatR;
using FluentResults;


namespace Cantina.Application.UseCase.User.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<string>>;
}
