namespace Benchmarks.App;

internal sealed class SpectreReportBuilder(IEnumerable<Summary> summaries)
{
    private IEnumerable<Summary> Summaries { get; } = summaries;

    // Default column benchmarks via BenchmarkConfig and summaries in their expected order
    private readonly string[] _defaultHeaders =
    [
        "Method",
        "Mean",
        "Median",
        "Error",
        "StdDev",
        "Op/s",
        "Gen0",
        "Gen1",
        "Gen2",
        "Allocated"
    ];

    public IRenderable Build()
    {
        var table = new Table
        {
            Border = TableBorder.Ascii2,
            BorderStyle = new Style(foreground: Color.Grey),
            UseSafeBorder = true
        };

        if (!Summaries.Any())
        {
            AnsiConsole.MarkupLine("[orange1]Warning:[/] No benchmark summaries found");
            return new Text(string.Empty);
        }

        foreach (var infoLine in Summaries.First().HostEnvironmentInfo.ToFormattedString())
        {
            SpectreLogger.Logger.WriteLine(infoLine);
        }

        AnsiConsole.WriteLine();

        var headers = BuildHeaders(table).ToArray();

        foreach (var summary in Summaries)
        {
            if (summary.Table.FullContent.Length == 0)
            {
                AnsiConsole.MarkupLine(
                $"[orange1]Warning:[/] No benchmark reports found [yellow]'{summary.Title}'[/]");
                continue;
            }

            BuildSummary(summary, table, headers);
        }

        return table;
    }

    // ReSharper disable once ReturnTypeCanBeEnumerable.Local - performance
    private string[] BuildHeaders(Table table)
    {
        var summaryHeaders = Summaries
            .SelectMany(s => s.Table.Columns)
            .Where(c => c.NeedToShow)
            .DistinctBy(c => c.Header)
            .Select(c => c.Header)
            .ToList();

        // First take the intersection of defaults and summaries, which ensures all columns are
        // included in their expected order, then append any remaining columns in summaries
        var headers = _defaultHeaders
            .Intersect(summaryHeaders)
            .Union(summaryHeaders)
            .Distinct()
            .ToArray();

        foreach (var header in headers)
        {
            table.AddColumn(header, cfg => cfg.RightAligned());
        }

        // Method
        if (table.Columns.Count > 0)
        {
            table.Columns[0].LeftAligned();
        }

        return headers;
    }

    private static void BuildSummary(Summary summary, Table table, string[] headers)
    {
        foreach (var line in summary.Table.FullContent)
        {
            var columns = new string[headers.Length];

            for (var i = 0; i < headers.Length; i++)
            {
                columns[i] = string.Empty;
            }

            for (var columnIndex = 0; columnIndex < summary.Table.ColumnCount; columnIndex++)
            {
                var column = summary.Table.Columns[columnIndex];
                if (!column.NeedToShow)
                {
                    continue;
                }

                var index = Array.IndexOf(headers, column.Header);
                var value = line[columnIndex];
                if (long.TryParse(value, out var longValue))
                {
                    value = longValue.ToString("N0");
                }
                columns[index] = $"[blue]{value}[/]";
            }

            table.AddRow(columns);
        }
    }
}
