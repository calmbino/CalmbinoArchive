using CalmbinoArchive.Infrastructure.Data;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.Extensions.Hosting;

namespace CalmbinoArchive.Infrastructure.Extensions;

public static class DatabaseExtentions
{
    public static IHostApplicationBuilder? AddPostgreDatabase(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<DataContext>("CalmbinoArchive",
            configureDbContextOptions: (options =>
            {
                options.UseExceptionProcessor();
                options.EnableSensitiveDataLogging();
            }));


        builder.AddRedisDistributedCache("cache");
        // builder.AddRedisClient("cache");

        return builder;
    }
}