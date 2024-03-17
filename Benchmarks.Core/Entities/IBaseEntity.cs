namespace Benchmarks.Core.Entities;

public interface IBaseEntity
{
    [MaxLength(42)]
    string Text { get; init; }
    DateTimeOffset CreatedAtUtc { get; init; }
    long LongInteger { get; init; }
}
