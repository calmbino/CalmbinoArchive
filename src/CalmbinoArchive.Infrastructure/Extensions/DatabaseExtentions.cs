using CalmbinoArchive.Infrastructure.Data;
using Microsoft.Extensions.Hosting;

namespace CalmbinoArchive.Infrastructure.Extensions;

public static class DatabaseExtentions
{
    public static IHostApplicationBuilder? AddPostgreDatabase(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<DataContext>("CalmbinoArchive");

        return builder;
    }
}