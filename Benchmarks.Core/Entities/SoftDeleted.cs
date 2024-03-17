namespace Benchmarks.Core.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; init; }

    DateTimeOffset? DeletedAtUtc { get; init; }
}

public abstract class LongPrimaryKeyBase : IBaseEntity, ILongPrimaryKey
{
    public long Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;
    public long LongInteger { get; init; }
}

public sealed class SoftDeleted : LongPrimaryKeyBase, ISoftDeletable
{
    public bool IsDeleted { get; init; }
    public DateTimeOffset? DeletedAtUtc { get; init; }
}

public sealed class HardDeleted : LongPrimaryKeyBase;
