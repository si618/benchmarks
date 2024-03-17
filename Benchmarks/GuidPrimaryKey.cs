namespace Benchmarks;

using GuidPrimaryKeyEntity = Benchmarks.Core.Entities.GuidPrimaryKey;

[BenchmarkInfo(
    description: "Benchmark using GUID based primary keys",
    links: [
        "https://youtu.be/n17U7ntLMt4?si=lFUX24PlGOQrtIKR",
        "https://blog.novanet.no/careful-with-guid-as-clustered-index"
    ],
    Category.Database)]
public class GuidPrimaryKey
{
    [Params(1_000, 10_000)]
    public int RowCount { get; set; }

    private readonly MsSqlContainer _sqlServerContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:latest")
        .Build();
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    [GlobalSetup]
    public async Task Setup()
    {
        await _postgresContainer.StartAsync();
        await _sqlServerContainer.StartAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyPostgres()
    {
        var repository = new GuidPrimaryKeyRepository(
            DbServer.Postgres,
            _postgresContainer.GetConnectionString());
        await repository.InsertEntitiesAsync<GuidPrimaryKeyEntity>(RowCount);
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithClusteredIndexSqlServer()
    {
        var repository = new GuidPrimaryKeyRepository(
            DbServer.SqlServer,
            _sqlServerContainer.GetConnectionString());
        await repository.InsertEntitiesAsync<ClusteredIndex>(RowCount);
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithNonClusteredIndexSqlServer()
    {
        var repository = new GuidPrimaryKeyRepository(
            DbServer.SqlServer,
            _sqlServerContainer.GetConnectionString());
        await repository.InsertEntitiesAsync<NonClusteredIndex>(RowCount);
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _postgresContainer.StopAsync();
        await _sqlServerContainer.StopAsync();
    }
}
