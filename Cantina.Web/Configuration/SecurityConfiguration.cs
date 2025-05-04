using Cantina.Domain.Entities;
using Cantina.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cantina.Web.Configuration
{
    public static class SecurityConfiguration
    {
        public static IServiceCollection AddCantinaSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization();
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                            .AddJwtBearer(options => {
                                options.RequireHttpsMetadata = false;
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                                    ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:SecretKey")))
                                };
                            });
            return services;
        }

        public static IServiceCollection AddCantinaIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                    options.Lockout.MaxFailedAccessAttempts = configuration.GetValue<int>("Lockout:MaxFailedAccessAttempts");
                    options.Lockout.AllowedForNewUsers = configuration.GetValue<bool>("Lockout:AllowedForNewUsers");
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(configuration.GetValue<int>("Lockout:DefaultLockoutTimeSpanInMinutes"));
                })
                .AddEntityFrameworkStores<CantinaDbContext>()
                .AddRoles<IdentityRole>();
            return services;
        }
    }
}
