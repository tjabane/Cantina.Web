using Cantina.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Cantina.Web.Extension
{
    public static class MigrationsExtension
    {
        public static IApplicationBuilder UseMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CantinaDbContext>();
                dbContext.Database.Migrate();
            }
            return app;
        }
    }
}
