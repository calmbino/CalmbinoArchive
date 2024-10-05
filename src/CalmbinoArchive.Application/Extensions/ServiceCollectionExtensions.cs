using Microsoft.Extensions.DependencyInjection;

namespace CalmbinoArchive.Application.Extentions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}