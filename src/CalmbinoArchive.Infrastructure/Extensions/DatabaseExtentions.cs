using CalmbinoArchive.Application.Interfaces;
using CalmbinoArchive.Infrastructure.Data;
using CalmbinoArchive.Infrastructure.Services;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace CalmbinoArchive.Infrastructure.Extensions;

public static class DatabaseExtentions
{
    public static IHostApplicationBuilder? AddPostgreSql(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<DataContext>("CalmbinoArchive",
            configureDbContextOptions: (options =>
            {
                options.UseExceptionProcessor();
                options.EnableSensitiveDataLogging();
            }));


        return builder;
    }

    public static IHostApplicationBuilder? AddRedis(this IHostApplicationBuilder builder)
    {
        builder.AddRedisClient("cache");

        return builder;
    }
}