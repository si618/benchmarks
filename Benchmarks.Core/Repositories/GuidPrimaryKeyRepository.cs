namespace Benchmarks.Core.Repositories;

public sealed class GuidPrimaryKeyRepository(DbServer dbServer, string connectionString)
{
    private static TEntity[] CreateEntities<TEntity>(int rowCount)
        where TEntity : GuidPrimaryKeyBase, new()
    {
        var entities = new TEntity[rowCount];

        // Worth benchmarking against Parallel.For implementation?
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

    public async Task InsertEntitiesAsync<TEntity>(int rowCount)
        where TEntity : GuidPrimaryKeyBase, new()
    {
        await using var dbContext = BenchmarkDbContextFactory.Create(dbServer, connectionString);
        await dbContext.Database.MigrateAsync();
        var entities = CreateEntities<TEntity>(rowCount);
        await dbContext.Set<TEntity>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }
}
