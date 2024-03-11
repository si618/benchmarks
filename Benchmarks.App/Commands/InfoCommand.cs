namespace Benchmarks.App.Commands;

internal sealed class InfoCommand : Command<BenchmarkRunSettings>
{
    [SuppressMessage("ReSharper", "RedundantNullableFlowAttribute")]
    public override int Execute(
        [NotNull] CommandContext context,
        [NotNull] BenchmarkRunSettings settings)
    {
        ConsoleWriter.WriteHeader();

        if (!Reflection.TryGetBenchmark(settings.Name, out var benchmark))
        {
            throw new InvalidOperationException("Run benchmark settings validation failed");
        }

        AnsiConsole.Write(benchmark.Markup());

        AnsiConsole.WriteLine();

        return 0;
    }
}
