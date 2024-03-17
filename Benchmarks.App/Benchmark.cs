namespace Benchmarks.App;

using Core.Benchmarking;

internal record Benchmark(
    string Name,
    string? Description = null,
    Uri[]? Links = null,
    Category Category = Category.None)
{
    public string Name { get; init; } = Name;
    public string Description { get; init; } = Description ?? string.Empty;
    public Uri[] Links { get; init; } = Links is null
        ? []
        : Links.Select(link => new Uri($"{link}", UriKind.Absolute)).ToArray();
    public Category Category { get; init; } = Category;
}

internal static class BenchmarkExtensions
{
    internal static Table Markup(this Benchmark benchmark)
    {
        var table = new Table
        {
            Border = TableBorder.None,
            ShowHeaders = false
        };

        table.AddColumn(new TableColumn("-").PadRight(3));
        table.AddColumn("-");

        table.AddRow("  [gray]Benchmark[/]", benchmark.Name);

        if (benchmark.Name != benchmark.Description &&
            !string.IsNullOrWhiteSpace(benchmark.Description))
        {
            table.AddRow("  [gray]Description[/]", benchmark.Description);
        }

        foreach (var link in benchmark.Links)
        {
            table.AddRow("  [gray]Link[/]", link.ToString());
        }

        if (benchmark.Category is not Category.None)
        {
            table.AddRow("  [gray]Category[/]", benchmark.Category.Description());
        }

        return table;
    }
}
