using System.Threading.RateLimiting;

namespace Cantina.Web.Configuration
{
    public static class RateLimiteConfiguration
    {
        public static IServiceCollection AddCantinaRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRateLimiter(options =>
            {

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                                RateLimitPartition.GetFixedWindowLimiter(
                                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                                    factory: partition => new FixedWindowRateLimiterOptions
                                    {
                                        AutoReplenishment = configuration.GetValue<bool>("RateLimit:AutoReplenishment"),
                                        PermitLimit = configuration.GetValue<int>("RateLimit:PermitLimit"),
                                        QueueLimit = configuration.GetValue<int>("RateLimit:QueueLimit"),
                                        Window = TimeSpan.FromMinutes(configuration.GetValue<int>("RateLimit:ReplenishmentPeriodInMinutes"))
                                 }));
            });
            return services;
        }
    }
}
