using CalmbinoArchive.Web.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace CalmbinoArchive.Web.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebShared(this IServiceCollection services)
    {
        // Add MudBlazor Services
        services.AddMudServices();

        try
        {
            // Add Http Client
            // services.ConfigureHttpClientDefaults(static http => { http.AddServiceDiscovery(); });
            services.AddHttpClient("CalmbinoArchive-Api",
                static client => client.BaseAddress = new Uri("https://localhost:7245"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        // Dependency Injection
        services.AddScoped<WeatherForecastService>();


        return services;
    }
}