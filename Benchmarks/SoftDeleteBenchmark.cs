namespace Benchmarks;

[BenchmarkInfo(
    description: "Benchmark hard verses soft deletes",
    links:
    [
        "https://www.milanjovanovic.tech/blog/implementing-soft-delete-with-ef-core",
        "https://blog.jetbrains.com/dotnet/2023/06/14/how-to-implement-a-soft-delete-strategy-with-entity-framework-core"
    ],
    Category.Database)]
public class SoftDeleteBenchmark : BenchmarkDbBase
{
    [Params(1_000)]
    public int RowCount { get; set; }

    [Params(DbServer.Postgres/*, DbServer.SqlServer*/)]
    public DbServer DbServer { get; set; }

    private SoftDeleteRepository CreateRepository(DbServer dbServer) =>
        new(BenchmarkDbContextFactory.Create(dbServer, dbServer switch
        {
            DbServer.Postgres => Postgres.GetConnectionString(),
            DbServer.SqlServer => SqlServer.GetConnectionString(),
            _ => throw dbServer.InvalidEnumArgumentException()
        }));

    [Benchmark]
    public async Task HardDelete()
    {
        var repository = CreateRepository(DbServer);
        await repository.MigrateAsync();
        await repository.CreateAsync<HardDelete>(RowCount);
        await repository.DeleteAsync<HardDelete>(RowCount);
    }

    [Benchmark]
    public async Task SoftDeleteWithIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.MigrateAsync();
        await repository.CreateAsync<SoftDeleteWithIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithIndexFilter>(RowCount);
    }

    [Benchmark]
    public async Task SoftDeleteWithoutIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.CreateAsync<SoftDeleteWithoutIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithoutIndexFilter>(RowCount);
    }

    [Benchmark]
    public async Task SelectAllWithIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.CreateAsync<SoftDeleteWithIndexFilter>(RowCount);
        await repository.SelectAllAsync<SoftDeleteWithIndexFilter>();
    }

    [Benchmark]
    public async Task SelectAllWithoutIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.CreateAsync<SoftDeleteWithoutIndexFilter>(RowCount);
        await repository.SelectAllAsync<SoftDeleteWithoutIndexFilter>();
    }

    [Benchmark]
    public async Task SelectDeletedWithIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.CreateAsync<SoftDeleteWithIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithIndexFilter>(RowCount / 3);
        await repository.SelectDeletedAsync<SoftDeleteWithIndexFilter>();
    }

    [Benchmark]
    public async Task SelectDeletedWithoutIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.CreateAsync<SoftDeleteWithoutIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithIndexFilter>(RowCount / 3);
        await repository.SelectDeletedAsync<SoftDeleteWithoutIndexFilter>();
    }

    [Benchmark]
    public async Task SelectNotDeletedWithIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.CreateAsync<SoftDeleteWithIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithIndexFilter>(RowCount / 3);
        await repository.SelectNonDeletedAsync<SoftDeleteWithIndexFilter>();
    }

    [Benchmark]
    public async Task SelectNotDeletedWithoutIndexFilter()
    {
        var repository = CreateRepository(DbServer);
        await repository.CreateAsync<SoftDeleteWithoutIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithIndexFilter>(RowCount / 3);
        await repository.SelectNonDeletedAsync<SoftDeleteWithoutIndexFilter>();
    }
}
