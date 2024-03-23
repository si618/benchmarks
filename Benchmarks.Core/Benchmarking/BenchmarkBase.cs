namespace Benchmarks.Core.Benchmarking;

[ShortRunJob]
[MarkdownExporter]
public abstract class BenchmarkBase
{
    // Common code for all benchmarks goes here
}

public abstract class BenchmarkDbBase : BenchmarkBase
{
    protected readonly MsSqlContainer SqlServer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:latest")
        .Build();
    protected readonly PostgreSqlContainer Postgres = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    [GlobalSetup]
    public async Task Setup()
    {
        await Postgres.StartAsync();
        await SqlServer.StartAsync();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await Postgres.StopAsync();
        await SqlServer.StopAsync();
    }
}
