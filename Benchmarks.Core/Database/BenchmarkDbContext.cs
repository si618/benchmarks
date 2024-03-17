namespace Benchmarks.Core.Database;

public class BenchmarkDbContext(DbContextOptions options) : DbContext(options)
{
    // GuidPrimaryKey Benchmarks
    public DbSet<GuidPrimaryKeyWithClusteredIndex> ClusteredIndexes { get; set; } = null!;
    public DbSet<GuidPrimaryKey> GuidPrimaryKeys { get; set; } = null!;
    public DbSet<GuidPrimaryKeyWithNonClusteredIndex> NonClusteredIndexes { get; set; } = null!;

    // SoftDelete Benchmarks
    public DbSet<HardDelete> HardDeletes { get; set; } = null!;
    public DbSet<SoftDeleteWithFilter> SoftDeleteWithFilters { get; set; } = null!;
    public DbSet<SoftDeleteWithoutFilter> SoftDeleteWithoutFilters { get; set; } = null!;

    /// <summary>
    /// Adds the <see cref="SoftDeleteInterceptor"/> when configuring context
    /// </summary>
    /// <remarks>
    /// Not ideal that it impacts all benchmarks using this context, but should do so consistently,
    /// and less work than splitting context per benchmark
    /// </remarks>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());

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
        modelBuilder.Entity<SoftDeleteWithFilter>()
            .HasQueryFilter(e => !e.IsDeleted)
            .HasKey(p => p.Id);
        modelBuilder.Entity<SoftDeleteWithFilter>()
            .HasIndex(e => e.IsDeleted)
            .HasFilter("IsDeleted = 0");
        modelBuilder.Entity<SoftDeleteWithoutFilter>()
            .HasQueryFilter(e => !e.IsDeleted)
            .HasKey(p => p.Id);
    }
}
