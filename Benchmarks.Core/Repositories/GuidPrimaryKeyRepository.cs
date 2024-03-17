namespace Benchmarks.Core.Repositories;

public sealed class GuidPrimaryKeyRepository(BenchmarkDbContext dbContext)
    : RepositoryBase(dbContext)
{
    private static TEntity[] Create<TEntity>(int rowCount) where TEntity : GuidPrimaryKeyBase, new()
    {
        var entities = new TEntity[rowCount];

        for (var row = 1; row <= rowCount; row++)
        {
            var now = DateTimeOffset.UtcNow;

            entities[row - 1] = new TEntity
            {
                Id = Guid.NewGuid(),
                Text = $"Row {row:N0}",
                CreatedAtUtc = now,
                LongInteger = now.Ticks
            };
        }

        return entities;
    }

    public async Task InsertAsync<TEntity>(int rowCount) where TEntity : GuidPrimaryKeyBase, new()
    {
        await DbContext.Database.MigrateAsync(); // TODO Refactor
        var entities = Create<TEntity>(rowCount);
        await DbContext.Set<TEntity>().AddRangeAsync(entities);
        await DbContext.SaveChangesAsync();
    }
}
