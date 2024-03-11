namespace Benchmarks.Core;

[AttributeUsage(AttributeTargets.Class)]
public sealed class BenchmarkInfoAttribute(
    string description,
    string uri = "",
    Category category = Category.None) : Attribute
{
    /// <summary>Benchmark description</summary>
    public string Description { get; } = description;

    /// <summary>Link to benchmark information</summary>
    public Uri? Link { get; } = string.IsNullOrEmpty(uri) ? null : new Uri($"{uri}", UriKind.Absolute);

    public Category Category { get; } = category;
}
