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
    if (PreFlightCheck.IsNeeded(args))
    {
        PreFlightCheck.Run(args);
    }

    return await app.RunAsync(args);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);

    return -99;
}
