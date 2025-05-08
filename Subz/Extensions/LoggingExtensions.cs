using Serilog;
using Serilog.Events;

namespace Subz.Extensions;

public static class LoggingExtensions
{
    public static void InitLogsWithSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            .CreateLogger();
        builder.Host.UseSerilog();
    }
}
