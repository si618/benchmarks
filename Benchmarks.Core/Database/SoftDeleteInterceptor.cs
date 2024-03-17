namespace Benchmarks.Core.Database;

/// <summary>
/// Modifies deleted entities as soft (virtual) deletes instead of hard (literal) deletes
/// </summary>
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

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry is not { State: EntityState.Deleted, Entity: ISoftDeletable delete })
            {
                continue;
            }

            entry.State = EntityState.Modified;
            delete.IsDeleted = true;
            delete.DeletedAtUtc = DateTimeOffset.UtcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
