using Microsoft.AspNetCore.Identity;
using FluentValidation;
using StackExchange.Redis;
using Cantina.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Cantina.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cantina.Infrastructure.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Cantina.Application.Interface;
using Cantina.Application.UseCase.User.Commands.CreateUser;
using Cantina.Domain.Repositories;
using Cantina.Infrastructure.Repository;
using Cantina.Web.Extension;
using Cantina.Web.Exceptions;
using System.Threading.RateLimiting;
using Cantina.Infrastructure.Options;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IMenuAuditRepository, MenuAuditRepository>();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IUserManager, UserManagerWrapper>();

builder.Services.AddMediatRConfiguration();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Databases
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
builder.Services.AddDbContext<CantinaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("RedisIndices"));


//Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                 })
                .AddEntityFrameworkStores<CantinaDbContext>()
                .AddRoles<IdentityRole>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options => {
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
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                    };
                });

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});

// Open Telemetry
builder.ConfigOpenTelemetry();
builder.Services.AddHealthCheck(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMigrations();
}

app.UseHttpsRedirection();
app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.UseAuthentication();

app.UseAuthorization();
app.UseExceptionHandler();

app.UseStatusCodePages();

app.MapControllers();

app.Run();
