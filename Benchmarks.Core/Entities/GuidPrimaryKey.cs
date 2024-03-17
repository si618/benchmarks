namespace Benchmarks.Core.Entities;

public abstract class GuidPrimaryKeyBase : IBaseEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Text { get; init; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;
    public long LongInteger { get; init; }
}

public sealed class GuidPrimaryKey : GuidPrimaryKeyBase;

public sealed class ClusteredIndex : GuidPrimaryKeyBase;

public sealed class NonClusteredIndex : GuidPrimaryKeyBase;
