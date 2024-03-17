namespace Benchmarks.Core.Repositories;

public sealed class SoftDeleteRepository(BenchmarkDbContext dbContext) : RepositoryBase(dbContext)
{
    private static TEntity[] Create<TEntity>(int rowCount)
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

    public async Task InsertAsync<TEntity>(int rowCount)
        where TEntity : LongPrimaryKeyBase, new()
    {
        await DbContext.Database.MigrateAsync(); // TODO Refactor
        var entities = Create<TEntity>(rowCount);
        await DbContext.Set<TEntity>().AddRangeAsync(entities);
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(int rowCount)
        where TEntity : LongPrimaryKeyBase
    {
        var entities = DbContext.Set<TEntity>().Take(rowCount);
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
    /// Select <see cref="ISoftDeletable.IsDeleted"/> entities predicated by <paramref name="isDeleted"/>.
    /// </summary>
    /// <remarks>
    /// Returned entities have no change tracking.
    /// </remarks>
    public async Task<List<TEntity>> SelectByIsDeletedAsync<TEntity>(bool isDeleted)
        where TEntity : LongPrimaryKeyBase, ISoftDeletable
        => await DbContext.Set<TEntity>()
            .AsNoTracking()
            .Where(e => e.IsDeleted == isDeleted)
            .ToListAsync();
}
