using Cantina.Domain.Entities;
using FluentResults;
using MediatR;


namespace Cantina.Application.UseCase.Menu.Query.GetMenuItemById
{
    public record GetMenuItemByIdQuery(int Id) : IRequest<Result<MenuItem>>;
}
