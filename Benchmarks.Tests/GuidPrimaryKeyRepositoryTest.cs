namespace Benchmarks.Tests;

public class GuidPrimaryKeyRepositoryTest : RepositoryTestBase
{
    private GuidPrimaryKeyRepository CreateRepository(DbServer dbServer) =>
        new(CreateDbContext(dbServer));

    [Theory]
    [InlineData(DbServer.Postgres)]
    [InlineData(DbServer.SqlServer)]
    public async Task InsertAsync_OnPostgres_CreatesExpectedRowCount(DbServer dbServer)
    {
        // Arrange
        var repository = CreateRepository(dbServer);
        await using var db = CreateDbContext(dbServer);

        // Act
        await repository.InsertAsync<GuidPrimaryKey>(10);

        // Assert
        var guidPrimaryKeys = db.GuidPrimaryKeys.ToList();
        guidPrimaryKeys.Should().HaveCount(10);
    }
}
