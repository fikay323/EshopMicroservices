using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCarter();
        services.AddHealthChecks()
            .AddSqlServer(config.GetConnectionString("Database")!);
        services.AddExceptionHandler<CustomExceptionHandler>();
        return services;
    }


    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseExceptionHandler(options => {});
        return app;
    }
}