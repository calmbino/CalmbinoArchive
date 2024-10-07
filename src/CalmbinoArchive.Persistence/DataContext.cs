using Microsoft.EntityFrameworkCore;

namespace CalmbinoArchive.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
}