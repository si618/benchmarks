﻿namespace Benchmarks;

using Core.Benchmarking;
using Core.Database;

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
        var entities = SimpleEntity.Create<SimpleEntity>(RowCount);
        await dbContext.SimpleEntities.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithClusteredIndexSqlServer()
    {
        await using var dbContext = CreateDbContext(DbServer.SqlServer);
        await dbContext.Database.MigrateAsync();
        var entities = SimpleEntity.Create<ClusteredIndex>(RowCount);
        await dbContext.ClusteredIndexes.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKeyWithNonClusteredIndexSqlServer()
    {
        await using var dbContext = CreateDbContext(DbServer.SqlServer);
        await dbContext.Database.MigrateAsync();
        var entities = SimpleEntity.Create<NonClusteredIndex>(RowCount);
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
