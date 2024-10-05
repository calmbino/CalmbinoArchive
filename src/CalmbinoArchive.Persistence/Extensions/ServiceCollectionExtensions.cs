using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalmbinoArchive.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}