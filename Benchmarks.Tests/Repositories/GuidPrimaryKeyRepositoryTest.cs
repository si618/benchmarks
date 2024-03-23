namespace Benchmarks.Tests.Repositories;

public class GuidPrimaryKeyRepositoryTest : RepositoryTestBase
{
    private const int RowCount = 10;

    private GuidPrimaryKeyRepository CreateRepository(DbServer dbServer) =>
        new(CreateDbContext(dbServer));

    [Theory]
    [InlineData(DbServer.Postgres)]
    [InlineData(DbServer.SqlServer)]
    public async Task CreateAsync_OnPostgres_CreatesExpectedRowCount(DbServer dbServer)
    {
        // Arrange
        var repository = CreateRepository(dbServer);
        await repository.MigrateAsync();
        await using var db = CreateDbContext(dbServer);

        // Act
        await repository.CreateAsync<GuidPrimaryKey>(RowCount);

        // Assert
        var guidPrimaryKeys = db.GuidPrimaryKeys.ToList();
        guidPrimaryKeys.Should().HaveCount(RowCount);
    }
}
