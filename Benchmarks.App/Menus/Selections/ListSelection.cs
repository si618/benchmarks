namespace Benchmarks.App.Menus.Selections;

internal sealed record ListSelection : Selection
{
    internal ListSelection(int order) : base("List benchmarks", order)
    {
    }

    public override int Execute()
    {
        ConsoleWriter.WriteHeader(clearConsole: true);

        var benchmarks = Reflection
            .GetBenchmarksByCategory()
            .SelectMany(g => g)
            .ToList();

        var padding = new BenchmarkPadding(
            benchmarks.Max(benchmark => benchmark.Name.Length),
            benchmarks.Max(benchmark => benchmark.Description.Length),
            benchmarks.Max(benchmark => benchmark.Category.Description().Length));

        var prompt = new SelectionPrompt<Benchmark>()
            .AddChoices([.. benchmarks])
            .PageSize(16)
        .UseConverter(benchmark => ConvertBenchmark(benchmark, padding))
        .MoreChoicesText("[gray](Move up and down to reveal more benchmarks)[/]");

        // TODO Capture X (or whatever) to return to main menu instead of exit workaround
        // Would also be good to filter by keyboard input e.g. Esc to return to main menu
        prompt.AddChoice(new Benchmark("Exit"));

        var table = new Table { Border = TableBorder.Simple };
        table.AddColumns("Benchmark", "Description", "Category");
        table.Columns[0].Width = padding.NamePad;
        table.Columns[1].Width = padding.DescriptionPad;
        table.Columns[2].Width = padding.CategoryPad;

        AnsiConsole.Cursor.MoveUp();
        AnsiConsole.Write(table);

        var benchmark = AnsiConsole.Prompt(prompt);
        if (benchmark.Name != "Exit")
        {
            var benchmarkMenu = new BenchmarkMenu(benchmark);
            benchmarkMenu.Render();
        }

        return 0;
    }

    private sealed record BenchmarkPadding(int NamePad, int DescriptionPad, int CategoryPad);

    private static string ConvertBenchmark(Benchmark benchmark, BenchmarkPadding padding)
        => new StringBuilder()
            .Append($"{benchmark.Name.PadRight(padding.NamePad + 3)}")
            .Append($"{benchmark.Description.PadRight(padding.DescriptionPad + 3)}")
            .Append($"{benchmark.Category.Description().PadRight(padding.CategoryPad + 3)}")
            .ToString();
}