namespace Benchmarks.App.Menus.Selections;

internal sealed record RunSelection : Selection
{
    private Benchmark Benchmark { get; }

    private RunSelection(Benchmark benchmark, string name, int order)
        : base(name, order)
    {
        Benchmark = benchmark;
    }

    internal RunSelection(Benchmark Benchmark, int order)
        : this(Benchmark, "Run benchmark", order)
    {
    }

    public override int Execute()
    {
        ConsoleWriter.WriteHeader(clearConsole: true);

        if (BenchmarkRunner.IsDebugConfiguration(true))
        {
            return 1;
        }

        var settings = new BenchmarkSettings { Name = Benchmark.Name };
        var summaries = BenchmarkRunner.RunAndBuildSummaries(settings);
        var builder = new SpectreReportBuilder(summaries);
        var report = builder.Build();

        AnsiConsole.Write(report);

        ConsoleWriter.WaitForKeyPress();

        return 0;
    }
}