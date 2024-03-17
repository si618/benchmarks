namespace Benchmarks;

[BenchmarkInfo(
    description: "Benchmark hard verses soft deletes",
    links:
    [
        "https://www.milanjovanovic.tech/blog/implementing-soft-delete-with-ef-core",
        "https://blog.jetbrains.com/dotnet/2023/06/14/how-to-implement-a-soft-delete-strategy-with-entity-framework-core"
    ],
    Category.Database)]
public class SoftDelete : BenchmarkDbBase
{
    [Params(1_000, 10_000)]
    public int RowCount { get; set; }

    [Params(DbServer.Postgres, DbServer.SqlServer)]
    public DbServer DbServer { get; set; }

    private SoftDeleteRepository CreateRepository(DbServer dbServer) =>
        new(BenchmarkDbContextFactory.Create(dbServer, dbServer switch
        {
            DbServer.Postgres => Postgres.GetConnectionString(),
            DbServer.SqlServer => SqlServer.GetConnectionString(),
            _ => throw dbServer.InvalidEnumArgumentException()
        }));

    [Benchmark]
    public async Task InsertThenHardDelete()
    {
        var repository = CreateRepository(DbServer);
        await repository.InsertAsync<HardDelete>(RowCount);
        await repository.DeleteAsync<HardDelete>(RowCount);
    }

    [Benchmark]
    public async Task InsertThenSoftDeleteWithIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.InsertAsync<SoftDeleteWithIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithIndexFilter>(RowCount);
    }

    [Benchmark]
    public async Task InsertThenSoftDeleteWithoutIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.InsertAsync<SoftDeleteWithoutIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithoutIndexFilter>(RowCount);
    }

    [Benchmark]
    public async Task InsertThenListSoftDeleteWithIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.InsertAsync<SoftDeleteWithIndexFilter>(RowCount);
        await repository.SelectAllAsync<SoftDeleteWithIndexFilter>();
    }

    [Benchmark]
    public async Task InsertThenListSoftDeleteWithoutIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.InsertAsync<SoftDeleteWithoutIndexFilter>(RowCount);
        await repository.SelectAllAsync<SoftDeleteWithoutIndexFilter>();
    }
}
