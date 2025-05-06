using Serilog;

namespace AzureCognitiveIntegration.Core.Extensions;

/// <summary>
/// SerilogExtension
/// </summary>
public static class SerilogExtension
{

    /// <summary>
    /// AddLoggingService
    /// </summary>
    /// <param name="builder"></param>
    public static void AddLoggingService(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((ctx, services, config) =>
        {
            config.ReadFrom.Configuration(ctx.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
                ;
        });
    }
}