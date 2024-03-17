namespace Benchmarks.Core.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }

    DateTimeOffset? DeletedAtUtc { get; set; }
}

public abstract class LongPrimaryKeyBase : IBaseEntity, ILongPrimaryKey
{
    public long Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;
    public long LongInteger { get; init; }
}

public sealed class SoftDeleteWithFilter : LongPrimaryKeyBase, ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAtUtc { get; set; }
}

public sealed class SoftDeleteWithoutFilter : LongPrimaryKeyBase, ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAtUtc { get; set; }
}

public sealed class HardDelete : LongPrimaryKeyBase;
