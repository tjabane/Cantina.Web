using Cantina.Domain.Entities;
using FluentResults;
using MediatR;


namespace Cantina.Application.UseCase.Menu.Query.GetMenu
{
    public class GetMenuQuery : IRequest<Result<List<MenuItem>>> { }
}
