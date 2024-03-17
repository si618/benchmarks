namespace Benchmarks.Core.Database.Postgres;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options)
    : BenchmarkDbContext(options)
{
    /// <summary>Postgres specific configuration.</summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        base.OnConfiguring(optionsBuilder);

    /// <summary>Postgres specific model building.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // SoftDelete Benchmarks
        modelBuilder.Entity<SoftDeleteWithIndexFilter>()
            .HasIndex(e => e.IsDeleted)
            .HasFilter("'IsDeleted' = '0'");
    }
}
