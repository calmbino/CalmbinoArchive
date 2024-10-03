using Microsoft.Extensions.DependencyInjection;

namespace CalmbinoArchive.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        return services;
    }
}