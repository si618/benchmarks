namespace Benchmarks.Core;

public abstract class SimpleEntity
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTimeOffset DateTimeUtc { get; set; } = DateTimeOffset.UtcNow;
    public long LongInteger { get; set; }
}

public class ClusteredIndex : SimpleEntity;
public class NonClusteredIndex : SimpleEntity;
