namespace Benchmarks.Core.Domain;

public abstract class SimpleEntityBase
{
    public Guid Id { get; init; } = Guid.Empty;
    [MaxLength(42)]
    public string Text { get; init; } = string.Empty;
    public DateTimeOffset DateTimeUtc { get; init; } = DateTimeOffset.UtcNow;
    public long LongInteger { get; init; }
}

public class SimpleEntity : SimpleEntityBase;
public class ClusteredIndex : SimpleEntityBase;
public class NonClusteredIndex : SimpleEntityBase;
