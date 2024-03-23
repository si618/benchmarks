namespace Benchmarks.Core.Repositories;

public sealed class GuidPrimaryKeyRepository(BenchmarkDbContext dbContext)
    : RepositoryBase(dbContext)
{
    private static TEntity[] Create<TEntity>(int count, int index)
        where TEntity : GuidPrimaryKeyBase, new()
    {
        var entities = new TEntity[count];

        for (var row = 1; row <= count; row++)
        {
            var now = DateTimeOffset.UtcNow;

            entities[row - 1] = new TEntity
            {
                Id = Guid.NewGuid(),
                Text = $"Row {row + index:N0}",
                CreatedAtUtc = now,
                LongInteger = now.Ticks
            };
        }

        return entities;
    }

    public async Task CreateAsync<TEntity>(int rowCount, int chunkSize = 1_000)
        where TEntity : GuidPrimaryKeyBase, new()
    {
        if (rowCount < 1) return;

        var offset = 0;
        var chunks = Enumerable.Range(1, rowCount).Chunk(chunkSize);
        foreach (var chunk in chunks)
        {
            var entities = Create<TEntity>(chunk.Length, offset);
            await DbContext.Set<TEntity>().AddRangeAsync(entities);
            await DbContext.SaveChangesAsync();
            offset += chunkSize;
        }
    }
}
