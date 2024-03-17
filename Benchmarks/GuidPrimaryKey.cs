namespace Benchmarks;

using GuidPrimaryKeyEntity = Benchmarks.Core.Entities.GuidPrimaryKey;

[BenchmarkInfo(
    description: "Benchmark GUID based primary keys",
    links: [
        "https://youtu.be/n17U7ntLMt4?si=lFUX24PlGOQrtIKR",
        "https://blog.novanet.no/careful-with-guid-as-clustered-index"
    ],
    Category.Database)]
public class GuidPrimaryKey
{
    [Params(1_000, 10_000)]
    public int RowCount { get; set; }

    private readonly MsSqlContainer _sqlServer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:latest")
        .Build();
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    [GlobalSetup]
    public async Task Setup()
    {
        await _postgres.StartAsync();
        await _sqlServer.StartAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyPostgres()
    {
        var repository = new GuidPrimaryKeyRepository(DbServer.Postgres, _postgres.GetConnectionString());
        await repository.InsertAsync<GuidPrimaryKeyEntity>(RowCount);
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithClusteredIndexSqlServer()
    {
        var repository = new GuidPrimaryKeyRepository(DbServer.SqlServer, _sqlServer.GetConnectionString());
        await repository.InsertAsync<GuidPrimaryKeyWithClusteredIndex>(RowCount);
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithNonClusteredIndexSqlServer()
    {
        var repository = new GuidPrimaryKeyRepository(DbServer.SqlServer, _sqlServer.GetConnectionString());
        await repository.InsertAsync<GuidPrimaryKeyWithNonClusteredIndex>(RowCount);
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _postgres.StopAsync();
        await _sqlServer.StopAsync();
    }
}
