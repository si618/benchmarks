namespace Benchmarks.Core;

public class SimpleEntity
{
    public Guid Id { get; init; } = Guid.Empty;
    [MaxLength(42)]
    public string Text { get; init; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;
    public long LongInteger { get; init; }

    public static TEntity[] Create<TEntity>(int rowCount)
        where TEntity : SimpleEntity, new()
    {
        var entities = new TEntity[rowCount];

        for (var row = 1; row <= rowCount; row++)
        {
            var now = DateTimeOffset.UtcNow;

            entities[row - 1] = new TEntity
            {
                Id = Guid.NewGuid(),
                Text = $"Row {row:N0}",
                CreatedAtUtc = now,
                LongInteger = now.Ticks
            };
        }

        return entities;
    }
}

public class ClusteredIndex : SimpleEntity;

public class NonClusteredIndex : SimpleEntity;

public class SoftDeleted : SimpleEntity
{
    public bool IsDeleted { get; init; }
    public DateTimeOffset? DeletedAtUtc { get; init; }
}
