namespace Benchmarks.Core.Database;

public class BenchmarkDbContext(DbContextOptions options) : DbContext(options)
{
    // GuidPrimaryKey Benchmarks
    public DbSet<GuidPrimaryKeyWithClusteredIndex> ClusteredIndexes { get; set; } = null!;
    public DbSet<GuidPrimaryKey> GuidPrimaryKeys { get; set; } = null!;
    public DbSet<GuidPrimaryKeyWithNonClusteredIndex> NonClusteredIndexes { get; set; } = null!;

    // SoftDelete Benchmarks
    public DbSet<HardDelete> HardDeletes { get; set; } = null!;
    public DbSet<SoftDeleteWithIndexFilter> SoftDeleteWithIndexFilters { get; set; } = null!;
    public DbSet<SoftDeleteWithoutIndexFilter> SoftDeleteWithoutIndexFilters { get; set; } = null!;

    /// <summary>Database agnostic configuration.</summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());

    /// <summary>Database agnostic model building.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // GuidPrimaryKey Benchmarks
        modelBuilder.Entity<GuidPrimaryKeyWithClusteredIndex>()
            .HasKey(e => e.Id)
            .IsClustered();
        modelBuilder.Entity<GuidPrimaryKey>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<GuidPrimaryKeyWithNonClusteredIndex>()
            .HasKey(e => e.Id)
            .IsClustered(false);

        // SoftDelete Benchmarks
        modelBuilder.Entity<HardDelete>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<SoftDeleteWithIndexFilter>()
            .HasQueryFilter(e => !e.IsDeleted)
            .HasKey(p => p.Id);
        modelBuilder.Entity<SoftDeleteWithoutIndexFilter>()
            .HasQueryFilter(e => !e.IsDeleted)
            .HasKey(p => p.Id);
    }
}
