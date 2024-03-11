namespace Benchmarks.App.Commands;

internal sealed class BenchmarkCommand : Command<BenchmarkSettings>
{
    [SuppressMessage("ReSharper", "RedundantNullableFlowAttribute")]
    public override int Execute(
        [NotNull] CommandContext context,
        [NotNull] BenchmarkSettings settings)
    {
        if (BenchmarkRunner.IsDebugConfiguration(settings.Debug))
        {
            return 1;
        }

        AnsiConsole.WriteLine();

        var summaries = BenchmarkRunner.RunAndBuildSummaries(settings);
        var builder = new SpectreReportBuilder(summaries);
        var report = builder.Build();

        AnsiConsole.Write(report);

        AnsiConsole.WriteLine();

        return 0;
    }
}
