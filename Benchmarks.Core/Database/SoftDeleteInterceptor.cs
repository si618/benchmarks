namespace Benchmarks.Core.Database;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        eventData
            .Context
            .ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(e => e.State == EntityState.Deleted)
            .ToList()
            .ForEach(entity =>
            {
                entity.State = EntityState.Modified;
                entity.Entity.IsDeleted = true;
                entity.Entity.DeletedAtUtc = DateTimeOffset.UtcNow;
            });

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
