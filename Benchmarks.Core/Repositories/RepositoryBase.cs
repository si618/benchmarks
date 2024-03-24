namespace Benchmarks.Core.Repositories;

public abstract class RepositoryBase(BenchmarkDbContext dbContext)
{
    protected BenchmarkDbContext DbContext { get; } = dbContext;

    public async Task MigrateAsync() => await DbContext.Database.MigrateAsync();
}
