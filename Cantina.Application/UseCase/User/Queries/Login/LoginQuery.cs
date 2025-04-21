using FluentResults;
using MediatR;

namespace Cantina.Application.UseCase.User.Queries.Login
{
    public record LoginQuery(string Email, string Password) : IRequest<Result<string>>;
}
