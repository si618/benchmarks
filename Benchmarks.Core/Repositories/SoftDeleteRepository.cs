namespace Benchmarks.Core.Repositories;

public sealed class SoftDeleteRepository(DbServer dbServer, string connectionString)
{
    private static TEntity[] CreateEntities<TEntity>(int rowCount)
        where TEntity : LongPrimaryKeyBase, new()
    {
        var entities = new TEntity[rowCount];

        for (var row = 1; row <= rowCount; row++)
        {
            var now = DateTimeOffset.UtcNow;

            entities[row - 1] = new TEntity
            {
                Text = $"Row {row:N0}",
                CreatedAtUtc = now,
                LongInteger = now.Ticks
            };
        }

        return entities;
    }

    public async Task InsertEntitiesAsync<TEntity>(int rowCount)
        where TEntity : LongPrimaryKeyBase, new()
    {
        await using var dbContext = BenchmarkDbContextFactory.Create(dbServer, connectionString);
        await dbContext.Database.MigrateAsync();
        var entities = CreateEntities<TEntity>(rowCount);
        await dbContext.Set<TEntity>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    public async Task SoftDeleteEntitiesAsync(int rowCount)
    {
        await using var dbContext = BenchmarkDbContextFactory.Create(dbServer, connectionString);
        await dbContext.Database.MigrateAsync();
        var entities = dbContext.SoftDeletes.Take(rowCount);
        dbContext.SoftDeletes.RemoveRange(entities);
        await dbContext.SaveChangesAsync();
    }

    public async Task HardDeleteEntitiesAsync(int rowCount)
    {
        await using var dbContext = BenchmarkDbContextFactory.Create(dbServer, connectionString);
        await dbContext.Database.MigrateAsync();
        var entities = dbContext.HardDeletes.Take(rowCount);
        dbContext.HardDeletes.RemoveRange(entities);
        await dbContext.SaveChangesAsync();
    }
}
