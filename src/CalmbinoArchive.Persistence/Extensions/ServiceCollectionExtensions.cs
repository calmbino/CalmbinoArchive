using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalmbinoArchive.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Context Connection
        var connectionString = configuration.GetConnectionString("CalmbinoArchiveDb");
        services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}