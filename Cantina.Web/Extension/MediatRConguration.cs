using Cantina.Application.Behaviors;
using Cantina.Application.UseCase.User.Commands.CreateUser;
using MediatR;

namespace Cantina.Web.Extension
{
    public static class MediatRConguration
    {
        public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(Core.UseCase.Handlers.CreateMenuItemCommandHandler).Assembly);
                cfg.RegisterServicesFromAssemblies(typeof(CreateUserCommandHandler).Assembly);
                cfg.AddOpenBehavior(typeof(RequestResponseLoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            return services;
        }
    }
}
