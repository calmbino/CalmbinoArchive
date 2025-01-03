using Microsoft.EntityFrameworkCore;

namespace CalmbinoArchive.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}