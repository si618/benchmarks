namespace Benchmarks;

using GuidPrimaryKeyEntity = Benchmarks.Core.Entities.GuidPrimaryKey;

[BenchmarkInfo(
    description: "Benchmark indexing of GUID primary keys",
    links: [
        "https://youtu.be/n17U7ntLMt4?si=lFUX24PlGOQrtIKR",
        "https://blog.novanet.no/careful-with-guid-as-clustered-index"
    ],
    Category.Database)]
public class GuidPrimaryKey : BenchmarkDbBase
{
    [Params(1_000, 10_000)]
    public int RowCount { get; set; }

    private GuidPrimaryKeyRepository CreateRepository(DbServer dbServer) =>
        new(BenchmarkDbContextFactory.Create(dbServer, dbServer switch
        {
            DbServer.Postgres => Postgres.GetConnectionString(),
            DbServer.SqlServer => SqlServer.GetConnectionString(),
            _ => throw dbServer.InvalidEnumArgumentException()
        }));

    /// <summary>
    /// Separate benchmarks for each database as Postgres doesn't support clustered indexes.
    /// </summary>
    /// <remarks>
    /// See https://stackoverflow.com/questions/4796548/about-clustered-index-in-postgres.
    /// </remarks>
    [Benchmark]
    public async Task InsertGuidPrimaryKey_OnPostgres()
    {
        var repository = CreateRepository(DbServer.Postgres);
        await repository.InsertAsync<GuidPrimaryKeyEntity>(RowCount);
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKey_WithClusteredIndex_OnSqlServer()
    {
        var repository = CreateRepository(DbServer.SqlServer);
        await repository.InsertAsync<GuidPrimaryKeyWithClusteredIndex>(RowCount);
    }

    [Benchmark]
    public async Task InsertGuidPrimaryKey_WithNonClusteredIndex_OnSqlServer()
    {
        var repository = CreateRepository(DbServer.SqlServer);
        await repository.InsertAsync<GuidPrimaryKeyWithNonClusteredIndex>(RowCount);
    }
}
