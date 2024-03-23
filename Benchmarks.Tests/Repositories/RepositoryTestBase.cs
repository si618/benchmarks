namespace Benchmarks.Tests.Repositories;

/// <summary>
/// Repository test base to setup database containers.
/// </summary>
/// <remarks>
/// Typically a single abstraction for the different database containers would be used, but since
/// this project is always testing benchmarks against multiple database providers, for ease of
/// development they are explicitly defined.
/// </remarks>
public abstract class RepositoryTestBase : IAsyncLifetime
{
    private readonly MsSqlContainer _sqlServer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:latest")
        .Build();
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    protected BenchmarkDbContext CreateDbContext(DbServer dbServer) =>
        BenchmarkDbContextFactory.Create(dbServer, dbServer switch
        {
            DbServer.Postgres => _postgres.GetConnectionString(),
            DbServer.SqlServer => _sqlServer.GetConnectionString(),
            _ => throw dbServer.InvalidEnumArgumentException()
        });

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        await _sqlServer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
        await _sqlServer.DisposeAsync();
    }
}
