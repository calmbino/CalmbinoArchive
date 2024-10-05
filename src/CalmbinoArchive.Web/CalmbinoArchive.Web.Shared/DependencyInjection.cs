using CalmbinoArchive.Web.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CalmbinoArchive.Web.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddWebShared(this IServiceCollection services)
    {
        services.AddHttpClient("CalmbinoArchiveAPI",
            client => client.BaseAddress = new Uri("https://localhost:7234"));

        services.AddScoped<WeatherForecastService>();

        return services;
    }
}