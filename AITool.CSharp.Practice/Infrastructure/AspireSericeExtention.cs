using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AITool.CSharp.Practice.Infrastructure;

public static class AspireSericeExtention
{
    public static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services)
    {
        // Endpoint to the Aspire Dashboard / Grafana Tempo (目前測試用 Tempo 看就不需要管 unicode 的問題)
        var endpoint = "http://localhost:4317";

        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService("TelemetryAspireDashboardQuickstart");

        // Enable model diagnostics with sensitive data.
        AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

        var traceProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddSource("Microsoft.SemanticKernel*")
            .AddConsoleExporter(options => { options.Targets = OpenTelemetry.Exporter.ConsoleExporterOutputTargets.Console; })
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(endpoint);
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            })
            .Build();

        var meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter("Microsoft.SemanticKernel*")
            .AddOtlpExporter(options => options.Endpoint = new Uri(endpoint))
            .Build();

        services.AddSingleton(traceProvider);
        services.AddSingleton(meterProvider);

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(resourceBuilder);
                options.AddOtlpExporter(options => options.Endpoint = new Uri(endpoint));
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
            });
            loggingBuilder.SetMinimumLevel(LogLevel.Information);
        });

        return services;
    }
}
