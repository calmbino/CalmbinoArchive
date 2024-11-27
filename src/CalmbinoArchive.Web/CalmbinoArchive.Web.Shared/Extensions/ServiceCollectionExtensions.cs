using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace CalmbinoArchive.Web.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebShared(this IServiceCollection services)
    {
        // Add MudBlazor Services
        services.AddMudServices();

        return services;
    }
}