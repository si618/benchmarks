namespace Benchmarks.App.Commands;

internal sealed class InfoCommand : Command<BenchmarkSettings>
{
    public override int Execute(CommandContext context, BenchmarkSettings settings)
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
