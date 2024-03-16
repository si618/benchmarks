namespace Benchmarks;

[BenchmarkInfo(
    description: "Test performance of GUID based primary keys",
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

    private BenchmarkDbContext CreateDbContext(DbServer server) =>
        BenchmarkDbContextFactory.Create(server, server switch
        {
            DbServer.Postgres => _postgresContainer.GetConnectionString(),
            DbServer.SqlServer => _sqlServerContainer.GetConnectionString(),
            _ => throw new NotImplementedException()
        });

    private static TEntity[] CreateEntities<TEntity>(int rowCount)
        where TEntity : SimpleEntityBase, new()
    {
        var entities = new TEntity[rowCount];

        for (var row = 1; row <= rowCount; row++)
        {
            var now = DateTimeOffset.UtcNow;

            entities[row - 1] = new TEntity
            {
                Id = Guid.NewGuid(),
                Text = $"Row {row:N0}",
                DateTimeUtc = now,
                LongInteger = now.Ticks
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
        await using var dbContext = CreateDbContext(DbServer.Postgres);
        await dbContext.Database.MigrateAsync();
        var entities = CreateEntities<SimpleEntity>(RowCount);
        await dbContext.SimpleEntities.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithClusteredIndexSqlServer()
    {
        await using var dbContext = CreateDbContext(DbServer.SqlServer);
        await dbContext.Database.MigrateAsync();
        var entities = CreateEntities<ClusteredIndex>(RowCount);
        await dbContext.ClusteredIndexes.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithNonClusteredIndexSqlServer()
    {
        await using var dbContext = CreateDbContext(DbServer.SqlServer);
        await dbContext.Database.MigrateAsync();
        var entities = CreateEntities<NonClusteredIndex>(RowCount);
        await dbContext.NonClusteredIndexes.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _postgresContainer.StopAsync();
        await _sqlServerContainer.StopAsync();
    }
}
