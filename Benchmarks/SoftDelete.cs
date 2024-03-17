namespace Benchmarks;

[BenchmarkInfo(
    description: "Test performance of soft delete operations",
    links: ["https://www.milanjovanovic.tech/blog/implementing-soft-delete-with-ef-core"],
    Category.Database)]
public class SoftDelete
{
    [Params(1_000, 10_000)]
    public int RowCount { get; set; }

    [Params(DbServer.Postgres, DbServer.SqlServer)]
    public DbServer DbServer { get; set; }

    private readonly MsSqlContainer _sqlServerContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:latest")
        .Build();
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    private BenchmarkDbContext CreateDbContext(DbServer server) =>
        BenchmarkDbContextFactory.Create(server, server switch
        {
            DbServer.Postgres => _postgresContainer.GetConnectionString(),
            DbServer.SqlServer => _sqlServerContainer.GetConnectionString(),
            _ => throw new InvalidEnumArgumentException()
        });

    [GlobalSetup]
    public async Task Setup()
    {
        await _postgresContainer.StartAsync();
        await _sqlServerContainer.StartAsync();
    }

    [Benchmark]
    public async Task SaveHardDelete()
    {
        await using var dbContext = CreateDbContext(DbServer);
    }

    [Benchmark]
    public async Task SaveSoftDelete()
    {
        await using var dbContext = CreateDbContext(DbServer);
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _postgresContainer.StopAsync();
        await _sqlServerContainer.StopAsync();
    }
}
