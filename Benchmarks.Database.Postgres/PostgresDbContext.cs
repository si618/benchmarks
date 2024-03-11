namespace Benchmarks.Database.Postgres;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options)
    : BenchmarkDbContext(options)
{

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}
