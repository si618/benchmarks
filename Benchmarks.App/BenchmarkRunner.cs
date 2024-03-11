namespace Benchmarks.App;

internal static class BenchmarkRunner
{
    public static IEnumerable<Summary> RunAndBuildSummaries(ListSettings settings)
    {
        var args = settings.BuildArgs();
        if (args.Length < 2)
        {
            // ReSharper disable once LocalizableElement - Exceptions in English
            throw new ArgumentOutOfRangeException(nameof(settings), "Invalid arguments");
        }

        AnsiConsole.Cursor.Move(CursorDirection.Up, 1);

        Console.SetOut(TextWriter.Null);

        var summaries = new List<Summary>();
        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start(WaitingMessage(settings, args), _ =>
                summaries.AddRange(
                    RunBenchmarks(ListSettings.BenchmarkTypes(), args)));

        Console.SetOut(Console.Out);

        AnsiConsole.Cursor.Move(CursorDirection.Down, 1);

        return summaries;
    }

    public static IEnumerable<Summary> RunBenchmarks(Type[] types, string[] args) =>
        BenchmarkSwitcher.FromTypes(types).Run(args);

    internal static bool IsDebugConfiguration(bool warnRatherThanFail = false)
    {
        // ReSharper disable once JoinDeclarationAndInitializer
        bool debug;

#if DEBUG
        debug = true;
#else
        debug = false;
#endif

        if (!debug)
        {
            return false;
        }

        if (warnRatherThanFail)
        {
            AnsiConsole.MarkupLine(
                "[orange1]Warning:[/] App should be in [yellow]RELEASE[/] configuration to run benchmarks");

            AnsiConsole.WriteLine();

            return false;
        }

        AnsiConsole.MarkupLine(
            "[red]Error:[/] App must be in [yellow]RELEASE[/] configuration to run benchmarks");

        AnsiConsole.WriteLine();

        return true;
    }

    private static string WaitingMessage(ListSettings settings, IReadOnlyList<string> args)
    {
        var message = new StringBuilder();

        if (args.Count > 2)
        {
            message.Append("Running selected benchmarks");
        }
        else if (Reflection.TryGetBenchmark(ParseFilterForBenchmark(args[1]), out var benchmark))
        {
            message.Append($"Running {benchmark} for [yellow]{benchmark.Description}[/]");

            if (!benchmark.HasSimilarNameAndDescription())
            {
                message.Append($" [gray](method: {benchmark.Name})[/]");
            }
        }
        else
        {
            message.Append("Running benchmarks");

            if (settings.Filter?.Length > 0)
            {
                message.Append($" matching [yellow]{settings.Filter!}[/]");
            }
        }

        return message.ToString();
    }

    private static string ParseFilterForBenchmark(string filter)
    {
        var benchmark = filter.Trim('*');
        var found = Reflection.GetBenchmarkNames()
            .FirstOrDefault(b => b.Equals(benchmark, StringComparison.OrdinalIgnoreCase));
        return found ?? filter;
    }
}
