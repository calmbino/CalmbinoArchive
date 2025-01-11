using CalmbinoArchive.Application.Interfaces.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace CalmbinoArchive.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}