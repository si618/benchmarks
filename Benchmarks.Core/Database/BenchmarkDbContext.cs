namespace Benchmarks.Core.Database;

public class BenchmarkDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ClusteredIndex> ClusteredIndexes { get; set; } = null!;
    public DbSet<GuidPrimaryKey> GuidPrimaryKeys { get; set; } = null!;
    public DbSet<HardDeleted> HardDeletes { get; set; } = null!;
    public DbSet<NonClusteredIndex> NonClusteredIndexes { get; set; } = null!;
    public DbSet<SoftDeleted> SoftDeletes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClusteredIndex>()
            .HasKey(p => p.Id)
            .IsClustered();

        modelBuilder.Entity<GuidPrimaryKey>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<HardDeleted>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<NonClusteredIndex>()
            .HasKey(p => p.Id)
            .IsClustered(false);

        modelBuilder.Entity<SoftDeleted>()
            .HasKey(p => p.Id);

    }
}
