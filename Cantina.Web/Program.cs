using Cantina.Core.Validator;
using Microsoft.AspNetCore.Identity;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using FluentValidation;
using Cantina.Core.Dto;
using Cantina.Infrastructure.MessageBroker;
using Cantina.Core.Interface;
using Cantina.Infrastructure.Redis;
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
using Cantina.Application.Behaviors;
using System.Reflection;
using Cantina.Web.Exceptions;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMenuQueryRepository, MenuQueryRepository>();
builder.Services.AddSingleton<IReviewQueryRepository, ReviewQueryRepository>();


builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IUserManager, UserManagerWrapper>();

builder.Services.AddMediatRConfiguration();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
// Message Broker 
builder.Services.AddSingleton<IMenuCommandRepository>(sp =>
{
    var host = builder.Configuration["MessageBroker:Server"];
    var topic = builder.Configuration["MessageBroker:Topic"];
    return new MenuCommandRepository(host, topic);
});

builder.Services.AddSingleton<IReviewCommandRepository>(sp =>
{
    var host = builder.Configuration["MessageBroker:Server"];
    var topic = builder.Configuration["MessageBroker:Topic"];
    return new ReviewCommandRepository(host, topic);
});

// Configure Options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Entity Framework
builder.Services.AddDbContext<CantinaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMigrations();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.UseExceptionHandler();

app.UseStatusCodePages();

app.MapControllers();

app.Run();
