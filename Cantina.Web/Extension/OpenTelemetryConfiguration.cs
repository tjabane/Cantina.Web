using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Cantina.Web.Extension
{
    public static class OpenTelemetryConfiguration
    {
        public static WebApplicationBuilder ConfigOpenTelemetry(this WebApplicationBuilder builder)
        {
            var applicationName = builder.Configuration["ApplicationName"] ?? "The Cantina";
            var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
            builder.Logging.AddOpenTelemetry(options =>
            {
                options
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(applicationName))
                    .AddConsoleExporter();
            });
            var otel = builder.Services.AddOpenTelemetry();
            otel.ConfigureResource(resource => resource.AddService(serviceName: applicationName));
            otel.WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddMeter("Microsoft.AspNetCore.Hosting")
                .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                .AddMeter("System.Net.Http")
                .AddMeter("System.Net.NameResolution")
                .AddConsoleExporter()
                .AddPrometheusExporter());
            otel.WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                tracing.AddSqlClientInstrumentation();
                tracing.AddEntityFrameworkCoreInstrumentation();
                if (tracingOtlpEndpoint is not null)
                    tracing.AddOtlpExporter(otlpOptions => otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint));
                else
                    tracing.AddConsoleExporter();
            });
            return builder;
        }
    }
}
