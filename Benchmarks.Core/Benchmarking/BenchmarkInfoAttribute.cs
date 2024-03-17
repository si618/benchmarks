namespace Benchmarks.Core.Benchmarking;

[AttributeUsage(AttributeTargets.Class)]
public sealed class BenchmarkInfoAttribute(
    string description,
    string[]? links = null,
    Category category = Category.None) : Attribute
{
    /// <summary>Benchmark description.</summary>
    public string Description { get; } = description;

    /// <summary>Links to benchmark information.</summary>
    public Uri[] Links { get; } = links is null
        ? []
        : links.Select(link => new Uri($"{link}", UriKind.Absolute)).ToArray();

    public Category Category { get; } = category;
}
