namespace Benchmarks.App;

internal record Benchmark(
    string Name,
    string Description = "",
    Uri? Link = null,
    Category Category = Category.None);

internal static partial class BenchmarkExtensions
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

        table.AddRow("  [gray]Benchmark[/]", benchmark.Name.TrimEnd("Benchmark".ToCharArray()));

        if (benchmark.Name != benchmark.Description &&
            !string.IsNullOrWhiteSpace(benchmark.Description))
        {
            table.AddRow("  [gray]Description[/]", benchmark.Description);
        }

        if (benchmark.Link is not null)
        {
            table.AddRow("  [gray]Link[/]", benchmark.Link.ToString());
        }

        if (benchmark.Category is not Category.None)
        {
            table.AddRow("  [gray]Category[/]", benchmark.Category.Description());
        }

        return table;
    }
}