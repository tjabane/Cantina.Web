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

var builder = WebApplication.CreateBuilder(args);
var applicationName = builder.Configuration["ApplicationName"] ?? "The Cantina";
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMenuQueryRepository, MenuQueryRepository>();
builder.Services.AddSingleton<IReviewQueryRepository, ReviewQueryRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddScoped<IValidator<MenuItem>, MenuItemValidator>();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IUserManager, UserManagerWrapper>();

//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Cantina.Core.UseCase.Handlers.GetMenuQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateUserCommandHandler).Assembly));

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


// Open Telemetry
builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(applicationName))
        .AddConsoleExporter();
});

builder.Services.AddOpenTelemetry()
      .ConfigureResource(resource => resource.AddService(applicationName))
      .WithTracing(tracing => tracing
          .AddAspNetCoreInstrumentation()
          .AddConsoleExporter())
      .WithMetrics(metrics => metrics
          .AddAspNetCoreInstrumentation()
          .AddConsoleExporter());

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
