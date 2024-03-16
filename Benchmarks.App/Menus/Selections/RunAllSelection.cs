namespace Benchmarks.App.Menus.Selections;

internal sealed record RunAllSelection : Selection
{
    internal RunAllSelection(int order) : base("Run benchmarks", order)
    {
    }

    public override int Execute()
    {
        ConsoleWriter.WriteHeader(clearConsole: true);

        if (BenchmarkRunner.IsDebugConfiguration(true))
        {
            return 1;
        }

        var summaries = BenchmarkRunner.RunAndBuildSummaries();
        var builder = new SpectreReportBuilder(summaries);
        var report = builder.Build();

        AnsiConsole.Write(report);

        ConsoleWriter.WaitForKeyPress();

        return 0;

    }
}
