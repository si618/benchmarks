namespace Benchmarks.App.Commands;

internal sealed class BenchmarkCommand : Command<ListSettings>
{
    [SuppressMessage("ReSharper", "RedundantNullableFlowAttribute")]
    public override int Execute(
        [NotNull] CommandContext context,
        [NotNull] ListSettings settings)
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
