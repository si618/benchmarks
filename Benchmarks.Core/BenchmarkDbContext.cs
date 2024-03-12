﻿namespace Benchmarks.Core;

public class BenchmarkDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ClusteredIndex> ClusteredIndexes { get; set; } = null!;
    public DbSet<NonClusteredIndex> NonClusteredIndexes { get; set; } = null!;
    public DbSet<SimpleEntity> SimpleEntities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClusteredIndex>()
            .HasKey(p => p.Id)
            .IsClustered();

        modelBuilder.Entity<NonClusteredIndex>()
            .HasKey(p => p.Id)
            .IsClustered(false);

        modelBuilder.Entity<SimpleEntity>()
            .HasKey(p => p.Id)
            .IsClustered(false);
    }
}