namespace Benchmarks.App;

internal static class PreFlightCheck
{
    private static readonly string[] NeedsPreFlightCheck = ["app", "benchmark", "workflow"];

    internal static bool IsNeeded(string[] args) =>
        args.Length > 0 &&                             // Not needed when showing app details
        args.All(arg => !arg.StartsWith('-')) && // Not needed for help or version options
        args.Any(arg => NeedsPreFlightCheck      // Not needed for info or list commands
            .Any(pf => arg.Contains(pf, StringComparison.OrdinalIgnoreCase)));

    internal static void Run(IEnumerable<string> args)
    {
        var isApp = args.Any(arg => arg.Contains("app", StringComparison.OrdinalIgnoreCase));

        ConsoleWriter.WriteHeader();

        AnsiConsole.MarkupLine("[gray]Running pre-flight check to verify test containers build[/]");
        AnsiConsole.WriteLine();

        // Would be nice if ContainerBuilder.Validate method was public
        new ContainerBuilder()
            .WithImage("testcontainers/helloworld:latest")
            .WithPortBinding(8080, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
            .Build();

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[gray]Test container pre-flight check passed[/]");

        if (isApp)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1.42));
        }
    }
}
