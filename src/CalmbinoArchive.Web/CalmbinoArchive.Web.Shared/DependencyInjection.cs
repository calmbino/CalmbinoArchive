using Microsoft.Extensions.DependencyInjection;

namespace CalmbinoArchive.Web.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddWebShared(this IServiceCollection services)
    {
        services.AddHttpClient("CalmbinoArchiveAPI",
            client => client.BaseAddress = new Uri("https://localhost:7234"));

        return services;
    }
}