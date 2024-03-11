namespace Benchmarks.Core;

public class BenchmarkDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ClusteredIndex> ClusteredIndexes { get; set; } = null!;
    public DbSet<NonClusteredIndex> NonClusteredIndexes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ClusteredIndex>()
            .HasKey(e => e.Id);
        modelBuilder
            .Entity<ClusteredIndex>()
            .HasKey(p => p.Id)
            .IsClustered();

        modelBuilder
            .Entity<NonClusteredIndex>()
            .HasKey(e => e.Id);
        modelBuilder
            .Entity<NonClusteredIndex>()
            .HasKey(p => p.Id)
            .IsClustered(false);
    }
}
