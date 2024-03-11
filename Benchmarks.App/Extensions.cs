﻿namespace Benchmarks.App;

public static partial class Extensions
{
    internal static string Description(this Category category) =>
        category == Category.None ? string.Empty : category.ToString();

    internal static bool HasSimilarNameAndDescription(this Benchmark benchmark) =>
        benchmark.Description.ReplaceWhitespace(string.Empty) == benchmark.Name;

    internal static Table Markup(this Benchmark benchmark)
    {
        var table = new Table
        {
            Border = TableBorder.None,
            ShowHeaders = false
        };

        table.AddColumn(new TableColumn("-").PadRight(3));
        table.AddColumn("-");

        table.AddRow("[gray]Benchmark[/]", benchmark.Name.TrimEnd("Benchmark".ToCharArray()));

        if (benchmark.Name != benchmark.Description)
        {
            table.AddRow("[gray]Description[/]", benchmark.Description);
        }

        table.AddRow("[gray]Category[/]", benchmark.Category.Description());

        return table;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
    private static readonly Regex MatchWhitespace = WhitespaceRegex();
    private static string ReplaceWhitespace(this string input, string replacement) =>
        MatchWhitespace.Replace(input, replacement);
}
