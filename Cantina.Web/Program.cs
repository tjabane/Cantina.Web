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

var builder = WebApplication.CreateBuilder(args);
var applicationName = builder.Configuration["ApplicationName"] ?? "The Cantina";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Cantina.Core.UseCase.Handlers.GetMenuQueryHandler).Assembly));
builder.Services.AddSingleton<IMenuQueryRepository, MenuQueryRepository>();
builder.Services.AddScoped<IValidator<MenuItem>, MenuItemValidator>();
// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration);
});
// Message Broker
builder.Services.AddSingleton<IMenuCommandRepository>(sp =>
{
    var host = builder.Configuration["MessageBroker:Host"];
    var topic = builder.Configuration["MessageBroker:Topic"];
    return new MessageProducerClient(host, topic);
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
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
