namespace Cantina.Web.Extension
{
    public static class HealthCheckExtension
    {
        public static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnection = configuration.GetConnectionString("DefaultConnection");
            var redisConnection = configuration.GetConnectionString("Redis");
            services.AddHealthChecks().AddRedis(redisConnection, "redis")
                                      .AddSqlServer(sqlConnection);


        }
    }
}
