namespace Benchmarks.Core.Database.SqlServer;

public class SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
    : BenchmarkDbContext(options)
{
    /// <summary>SqlServer specific configuration.</summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        base.OnConfiguring(optionsBuilder);

    /// <summary>SqlServer specific model building.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // SoftDelete Benchmarks
        modelBuilder.Entity<SoftDeleteWithIndexFilter>()
            .HasIndex(e => e.IsDeleted)
            .HasFilter("[IsDeleted] = 0");
    }
}
