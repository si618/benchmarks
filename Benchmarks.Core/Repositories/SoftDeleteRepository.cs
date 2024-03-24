namespace Benchmarks.Core.Repositories;

public sealed class SoftDeleteRepository(BenchmarkDbContext dbContext) : RepositoryBase(dbContext)
{
    public async Task CreateAsync<TEntity>(int rowCount, int chunkSize = 1_000)
        where TEntity : LongPrimaryKeyBase, new()
    {
        if (rowCount < 1) return;

        var index = 0;
        var chunks = Enumerable.Range(1, rowCount).Chunk(chunkSize);
        foreach (var chunk in chunks)
        {
            var entities = Create<TEntity>(chunk.Length, index);
            await DbContext.Set<TEntity>().AddRangeAsync(entities);
            await DbContext.SaveChangesAsync();
            index += chunkSize;
        }
    }

    public async Task DeleteAsync<TEntity>(int deleteRowCount)
        where TEntity : LongPrimaryKeyBase
    {
        if (deleteRowCount < 1) return;
        var entities = DbContext.Set<TEntity>().Take(deleteRowCount);
        DbContext.Set<TEntity>().RemoveRange(entities);
        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// List all entities, including any that may be soft deleted.
    /// </summary>
    /// <remarks>
    /// Returned entities have no change tracking, and are not subject to query filters.
    /// </remarks>
    public async Task<List<TEntity>> SelectAllAsync<TEntity>()
        where TEntity : LongPrimaryKeyBase
        => await DbContext.Set<TEntity>()
            .AsNoTracking()
            .IgnoreQueryFilters() // Not sure about this; are method comments ok or too much magic?
            .ToListAsync();

    /// <summary>
    /// Select deleted entities.
    /// </summary>
    /// <remarks>
    /// Returned entities have no change tracking, and are not subject to query filters.
    /// </remarks>
    public async Task<List<TEntity>> SelectDeletedAsync<TEntity>()
        where TEntity : LongPrimaryKeyBase, ISoftDeletable
        => await SelectByIsDeletedAsync<TEntity>(true);

    /// <summary>
    /// Select non-deleted entities.
    /// </summary>
    /// <remarks>
    /// Returned entities have no change tracking, and are not subject to query filters.
    /// </remarks>
    public async Task<List<TEntity>> SelectNonDeletedAsync<TEntity>()
        where TEntity : LongPrimaryKeyBase, ISoftDeletable
        => await SelectByIsDeletedAsync<TEntity>(false);

    private static TEntity[] Create<TEntity>(int rowCount, int index)
        where TEntity : LongPrimaryKeyBase, new()
    {
        var entities = new TEntity[rowCount];

        for (var row = 1; row <= rowCount; row++)
        {
            var now = DateTimeOffset.UtcNow;

            entities[row - 1] = new TEntity
            {
                Text = $"Row {row + index:N0}",
                CreatedAtUtc = now,
                LongInteger = now.Ticks
            };
        }

        return entities;
    }

    private async Task<List<TEntity>> SelectByIsDeletedAsync<TEntity>(bool isDeleted)
        where TEntity : LongPrimaryKeyBase, ISoftDeletable
        => await DbContext.Set<TEntity>()
            .AsNoTracking()
            .IgnoreQueryFilters() // Not sure about this; are method comments ok or too much magic?
            .Where(e => e.IsDeleted == isDeleted)
            .ToListAsync();
}
