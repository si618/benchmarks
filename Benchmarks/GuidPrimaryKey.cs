namespace Benchmarks;

[BenchmarkInfo(
    "Test performance of GUID based primary keys",
    "https://blog.novanet.no/careful-with-guid-as-clustered-index",
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

    private TEntity[] CreateEntities<TEntity>() where TEntity : SimpleEntityBase, new()
    {
        var entities = new TEntity[RowCount];

        for (var row = 1; row <= RowCount; row++)
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
        await dbContext.SimpleEntities.AddRangeAsync(CreateEntities<SimpleEntity>());
        await dbContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithClusteredIndexSqlServer()
    {
        await using var dbContext = CreateDbContext(DbServer.SqlServer);
        await dbContext.Database.MigrateAsync();
        await dbContext.ClusteredIndexes.AddRangeAsync(CreateEntities<ClusteredIndex>());
        await dbContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithNonClusteredIndexSqlServer()
    {
        await using var dbContext = CreateDbContext(DbServer.SqlServer);
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
