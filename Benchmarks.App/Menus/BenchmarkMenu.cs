namespace Benchmarks.App.Menus;

internal sealed class BenchmarkMenu : MenuBase
{
    private Benchmark Benchmark { get; init; }

    public BenchmarkMenu(Benchmark benchmark)
    {
        Benchmark = benchmark;
        Choices =
        [
            new RunSelection(Benchmark, 1),
            new ExitSelection(2)
        ];
    }

    public override int Render()
    {
        ConsoleWriter.WriteHeader(clearConsole: true);

        AnsiConsole.Write(Benchmark.Markup());
        AnsiConsole.WriteLine();

        var prompt = new SelectionPrompt<Selection>()
            .AddChoices(GetChoices())
            .UseConverter(m => m.Name);

        var selected = AnsiConsole.Prompt(prompt);
        var exitCode = selected.Execute();

        return exitCode;
    }
}
