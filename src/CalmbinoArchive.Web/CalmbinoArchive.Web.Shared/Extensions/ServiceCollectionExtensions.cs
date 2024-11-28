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

        // Add Http Client
        services.AddHttpClient("CalmbinoAPI", client => client.BaseAddress = new Uri("api"));

        // Dependency Injection
        services.AddScoped<WeatherForecastService>();


        return services;
    }
}