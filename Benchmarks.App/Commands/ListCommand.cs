namespace Benchmarks.App.Commands;

using Core.Benchmarking;

internal sealed class ListCommand : Command
{
    public override int Execute(CommandContext context)
    {
        ConsoleWriter.WriteHeader();

        AnsiConsole.Cursor.Move(CursorDirection.Up, 1);

        var table = new Table();

        table.AddColumn("Benchmark", config => config.NoWrap = true);
        table.AddColumn("Description");
        table.AddColumn("Category");
        table.SimpleBorder();
        table.BorderColor(Color.Grey);

        var categories = Reflection.GetBenchmarksByCategory();

        foreach (var category in categories)
        {
            foreach (var benchmark in category.ToArray())
            {
                table.AddRow(
                    benchmark.Name,
                    benchmark.Description,
                    benchmark.Category.Description());
            }
        }

        AnsiConsole.Write(table);

        return 0;
    }
}
