using Cantina.Infrastructure.Metrics;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Cantina.Web.Configuration
{
    public static class OpenTelemetryConfiguration
    {
        public static WebApplicationBuilder ConfigOpenTelemetry(this WebApplicationBuilder builder)
        {
            var applicationName = builder.Configuration["ApplicationName"] ?? "The Cantina";
            var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
            
            builder.Services.AddOpenTelemetry()
                                .ConfigureResource(resource => resource.AddService(serviceName: applicationName))
                                .WithLogging(logging => logging
                                    .AddConsoleExporter()
                                    .AddOtlpExporter(otlpOptions => otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint))
                                )
                                .WithMetrics(metrics => metrics
                                    .AddProcessInstrumentation()
                                    .AddRuntimeInstrumentation()
                                    .AddMeter($"{applicationName}.Metrics.ReviewsMeter")
                                    .AddMeter("Microsoft.AspNetCore.Hosting")
                                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                                    .AddMeter("Microsoft.AspNetCore.Http.Connections")
                                    .AddMeter("Microsoft.AspNetCore.Routing")
                                    .AddMeter("Microsoft.AspNetCore.Diagnostics")
                                    .AddMeter("Microsoft.AspNetCore.RateLimiting")
                                    .AddConsoleExporter()
                                    .AddPrometheusExporter()
                                    .AddOtlpExporter(otlpOptions => otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint))
                                )
                                .WithTracing(tracing => tracing
                                
                                    .AddAspNetCoreInstrumentation()
                                    .AddHttpClientInstrumentation()
                                    .AddSqlClientInstrumentation()
                                    .AddEntityFrameworkCoreInstrumentation()
                                    .AddConsoleExporter()
                                    .AddOtlpExporter(otlpOptions => otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint))
                                );

            return builder;
        }
    }
}
