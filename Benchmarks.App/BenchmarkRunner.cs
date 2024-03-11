﻿namespace Benchmarks.App;

internal static class BenchmarkRunner
{
    public static IEnumerable<Summary> RunAndBuildSummaries()
    {
        var args = new[] { "--filter", $"Benchmarks*" };
        var types = Reflection.GetBenchmarkTypes().ToArray();

        return RunAndBuildSummaries(types, args);
    }


    public static IEnumerable<Summary> RunAndBuildSummaries(ListSettings settings)
    {
        var args = settings.BuildArgs();
        var type = Reflection.GetBenchmarkTypes().First(type => type.Name == settings.Name);

        return RunAndBuildSummaries([type], args);
    }

    public static IEnumerable<Summary> RunAndBuildSummaries(Type[] benchmarkTypes, string[] args)
    {

        AnsiConsole.Cursor.Move(CursorDirection.Up, 1);

        Console.SetOut(TextWriter.Null);

        var summaries = new List<Summary>();
        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start(WaitingMessage(benchmarkTypes), _ =>
                summaries.AddRange(
                    RunBenchmarks(benchmarkTypes, args)));

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

    private static string WaitingMessage(Type[] benchmarkTypes)
    {
        var message = new StringBuilder();

        if (benchmarkTypes.Length > 1)
        {
            message.Append("Running benchmarks");
        }
        else if (Reflection.TryGetBenchmark(benchmarkTypes[0].Name, out var benchmark))
        {
            message.Append($"Running benchmark {benchmark.Name}");
        }

        return message.ToString();
    }
}
