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

        var first = true;
        var linkText = benchmark.Links.Length > 1 ? "Links" : "Link";
        foreach (var link in benchmark.Links)
        {
            var linkColumn = first ? $"  [gray]{linkText}[/]" : string.Empty;
            table.AddRow(linkColumn, link.ToString());
            first = false;
        }

        if (benchmark.Category is not Category.None)
        {
            table.AddRow("  [gray]Category[/]", benchmark.Category.Description());
        }

        return table;
    }
}
