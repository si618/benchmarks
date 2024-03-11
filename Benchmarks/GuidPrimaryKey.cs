namespace Benchmarks;

[BenchmarkInfo(
    "Test performance of GUID based primary keys",
    "https://blog.novanet.no/careful-with-guid-as-clustered-index",
    Category.Database)]
public class GuidPrimaryKey
{
    [Params(1_000, 1_000_000)]
    public int RowCount { get; set; }

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
            _ => throw new NotImplementedException()
        });

    private T[] CreateEntities<T>() where T : SimpleEntity, new()
    {
        var entities = new T[RowCount];

        for (var row = 1; row <= RowCount; row++)
        {
            var now = DateTimeOffset.UtcNow;

            entities[row - 1] = new T()
            {
                Id = Guid.NewGuid(),
                Text = $"Row {row}",
                DateTimeUtc = now,
                LongInteger = now.Ticks,
                Decimal = now.Ticks / row
            };
        }

        return entities;
    }

    [GlobalSetup]
    public async Task Setup()
    {
        await _postgresContainer.StartAsync();
        await _sqlServerContainer.StartAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyPostgres()
    {
        using var dbContext = CreateDbContext(DbServer.Postgres);
        await dbContext.Database.MigrateAsync();
        await dbContext.ClusteredIndexes.AddRangeAsync(CreateEntities<ClusteredIndex>());
        await dbContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithClusteredIndexSqlServer()
    {
        using var dbContext = CreateDbContext(DbServer.SqlServer);
        await dbContext.Database.MigrateAsync();
        await dbContext.ClusteredIndexes.AddRangeAsync(CreateEntities<ClusteredIndex>());
        await dbContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithNonClusteredIndexSqlServer()
    {
        using var dbContext = CreateDbContext(DbServer.SqlServer);
        await dbContext.Database.MigrateAsync();
        await dbContext.NonClusteredIndexes.AddRangeAsync(CreateEntities<NonClusteredIndex>());
        await dbContext.SaveChangesAsync();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _postgresContainer.StopAsync();
        await _sqlServerContainer.StopAsync();
    }
}
