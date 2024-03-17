namespace Benchmarks.Core.Database.SqlServer;

public class SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
    : BenchmarkDbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}
