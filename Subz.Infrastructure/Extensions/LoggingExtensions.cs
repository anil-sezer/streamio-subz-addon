using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Subz.Infrastructure.Extensions;

public static class LoggingExtensions
{
    public static void InitLogsWithSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code, // or .Literate, .Grayscale, etc.
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            // .WriteTo.OpenTelemetry(
            //     endpoint: "http://127.0.0.1:4318/v1/logs",
            //     protocol: OtlpProtocol.Grpc
            //     )
            .CreateLogger();
        builder.Host.UseSerilog();
    }
}