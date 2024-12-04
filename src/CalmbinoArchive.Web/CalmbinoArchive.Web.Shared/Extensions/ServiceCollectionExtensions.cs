using CalmbinoArchive.Web.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;

namespace CalmbinoArchive.Web.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebShared(this IServiceCollection services)
    {
        // Add MudBlazor Services
        services.AddMudServices();
        services.AddMudMarkdownServices();

        // TODO: Find ways to leverage Aspire's Service Discovery
        services.AddScoped<WeatherClientService>();
        services.AddHttpClient<WeatherClientService>(
            static client => client.BaseAddress = new Uri("https://localhost:7245"));


        return services;
    }
}