namespace Benchmarks;

[BenchmarkInfo(
    description: "Benchmark hard verses soft deletes",
    links:
    [
        "https://www.milanjovanovic.tech/blog/implementing-soft-delete-with-ef-core",
        "https://blog.jetbrains.com/dotnet/2023/06/14/how-to-implement-a-soft-delete-strategy-with-entity-framework-core"
    ],
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

    private SoftDeleteRepository CreateRepository() => new(DbServer, DbServer switch
    {
        DbServer.Postgres => _postgresContainer.GetConnectionString(),
        DbServer.SqlServer => _sqlServerContainer.GetConnectionString(),
        _ => throw DbServer.InvalidEnumArgumentException()
    });

    [GlobalSetup]
    public async Task Setup()
    {
        await _postgresContainer.StartAsync();
        await _sqlServerContainer.StartAsync();
    }

    [Benchmark]
    public async Task HardDelete()
    {
        var repository = CreateRepository();
        await repository.InsertAsync<HardDelete>(RowCount);
        await repository.HardDeleteAsync(RowCount);
    }

    [Benchmark]
    public async Task SoftDeleteWithQueryFilter()
    {
        var repository = CreateRepository();
        await repository.InsertAsync<SoftDeleteWithFilter>(RowCount);
        await repository.SoftDeleteAsync<SoftDeleteWithFilter>(RowCount);
    }

    [Benchmark]
    public async Task SoftDeleteWithoutQueryFilter()
    {
        var repository = CreateRepository();
        await repository.InsertAsync<SoftDeleteWithoutFilter>(RowCount);
        await repository.SoftDeleteAsync<SoftDeleteWithoutFilter>(RowCount);
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _postgresContainer.StopAsync();
        await _sqlServerContainer.StopAsync();
    }
}
