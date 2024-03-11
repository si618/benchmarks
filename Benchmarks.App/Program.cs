var app = new CommandApp();

var exampleBenchmark = new[] { "benchmark", "GuidPrimaryKey" };
var exampleInfo = new[] { "info", "GuidPrimaryKey" };

app.Configure(config =>
{
    config.PropagateExceptions();
    config.SetApplicationName("Benchmark.exe");

    config.AddCommand<AppCommand>("app")
        .WithDescription("Run interactive console application");

    config.AddCommand<BenchmarkCommand>("benchmark")
        .WithDescription("Run benchmarks");

    config.AddCommand<InfoCommand>("info")
        .WithDescription("Show benchmark details");

    config.AddCommand<ListCommand>("list")
        .WithDescription("List benchmarks");

    config.AddCommand<WorkflowCommand>("workflow")
        .WithDescription("Run benchmarks for a GitHub workflow");

    config.AddExample(exampleBenchmark);
    config.AddExample(exampleInfo);
});

try
{
    // Do not run the preflight check if the user is asking for help or version information
    if (args.Length > 0 && !args.Any(a => a.StartsWith('-')))
    {
        PreFlightCheck.CanStartTestContainers();
    }

    return await app.RunAsync(args);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);

    return -99;
}
