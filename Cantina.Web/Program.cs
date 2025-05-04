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
using Cantina.Web.Configuration;
using Cantina.Infrastructure.Metrics;

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

builder.Services.AddSingleton<ReviewsMeter>();

// Databases
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
builder.Services.AddDbContext<CantinaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("RedisIndices"));

builder.Services.AddCantinaIdentity(configuration)
                .AddCantinaSecurity(configuration)
                .AddCantinaRateLimiting(configuration);

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
app.UseRateLimiter();
app.UseHttpsRedirection();
app.MapHealthChecks("/health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

app.UseAuthentication();

app.UseAuthorization();
app.UseExceptionHandler();

app.UseStatusCodePages();

app.MapControllers();

app.MapPrometheusScrapingEndpoint();

app.Run();
