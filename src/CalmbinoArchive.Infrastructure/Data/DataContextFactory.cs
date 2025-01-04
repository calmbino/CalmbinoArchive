using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CalmbinoArchive.Infrastructure.Data;

// For using "dotnet ef"
// https://learn.microsoft.com/ko-kr/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<DataContext>();

        optionBuilder.UseNpgsql("CalmbinoArchive");

        return new DataContext(optionBuilder.Options);
    }
}