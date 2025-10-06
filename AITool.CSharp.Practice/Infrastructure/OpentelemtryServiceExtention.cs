using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AITool.CSharp.Practice.Infrastructure;

public static class OpentelemtryServiceExtention
{
    /// <summary>
    /// Aspire / Grafana 快速開始 OpenTelemetry 設定
    /// </summary>
    /// <remarks>
    /// 1. Aspire 目前中文都會被 Unicode 過顯示不是很友善所以不使用<br/>
    /// 2. Grafana Tempo 目前測試可以正常顯示文字
    /// </remarks>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services)
    {
        // Endpoint to the Aspire Dashboard / Grafana Tempo 
        var endpoint = "http://localhost:4317";

        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService("TelemetryAspireDashboardQuickstart");

        // Enable model diagnostics with sensitive data.
        AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

        var traceProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddSource("Microsoft.SemanticKernel*")
            .AddConsoleExporter(options => { options.Targets = ConsoleExporterOutputTargets.Console; })
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(endpoint);
                options.Protocol = OtlpExportProtocol.Grpc;
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
