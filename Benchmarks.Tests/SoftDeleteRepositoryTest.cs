namespace Benchmarks.Tests;

public class SoftDeleteRepositoryTest : RepositoryTestBase
{
    private const int RowCount = 10;

    private SoftDeleteRepository CreateRepository(DbServer dbServer) =>
        new(CreateDbContext(dbServer));

    [Theory]
    [InlineData(DbServer.Postgres)]
    [InlineData(DbServer.SqlServer)]
    public async Task InsertAsync_InsertsRows_ForExpectedRowCount(DbServer dbServer)
    {
        // Arrange
        var repository = CreateRepository(dbServer);

        // Act
        await repository.InsertAsync<HardDelete>(RowCount);
        await repository.InsertAsync<SoftDeleteWithIndexFilter>(RowCount);

        // Assert
        var hardDeletes = await repository.SelectAllAsync<HardDelete>();
        var softDeletes = await repository.SelectAllAsync<SoftDeleteWithIndexFilter>();
        hardDeletes.Should().HaveCount(RowCount);
        softDeletes.Should().HaveCount(RowCount);
    }

    [Theory]
    [InlineData(DbServer.Postgres)]
    [InlineData(DbServer.SqlServer)]
    public async Task DeleteAsync_DeletesAllRows_WhenUsingHardDelete(DbServer dbServer)
    {
        // Arrange
        var repository = CreateRepository(dbServer);
        await repository.InsertAsync<HardDelete>(RowCount);

        // Act
        await repository.DeleteAsync<HardDelete>(RowCount);

        // Assert
        var hardDeletes = await repository.SelectAllAsync<HardDelete>();
        hardDeletes.Should().BeEmpty();
    }

    [Theory]
    [InlineData(DbServer.Postgres)]
    [InlineData(DbServer.SqlServer)]
    public async Task DeleteAsync_DeletesNoRows_WhenUsingSoftDeleteWithIndexFilter(DbServer dbServer)
    {
        // Arrange
        var repository = CreateRepository(dbServer);
        await repository.InsertAsync<SoftDeleteWithIndexFilter>(RowCount);

        // Act
        await repository.DeleteAsync<SoftDeleteWithIndexFilter>(RowCount);

        // Assert
        var softDeletes = await repository.SelectAllAsync<SoftDeleteWithIndexFilter>();
        softDeletes.Should()
            .HaveCount(RowCount)
            .And.AllSatisfy(e =>
            {
                e.IsDeleted.Should().BeTrue();
                e.DeletedAtUtc.Should().NotBeNull();
            });
    }

    [Theory]
    [InlineData(DbServer.Postgres)]
    [InlineData(DbServer.SqlServer)]
    public async Task DeleteAsync_DeletesNoRows_WhenUsingSoftDeleteWithoutIndexFilter(DbServer dbServer)
    {
        // Arrange
        var repository = CreateRepository(dbServer);
        await repository.InsertAsync<SoftDeleteWithoutIndexFilter>(RowCount);

        // Act
        await repository.DeleteAsync<SoftDeleteWithoutIndexFilter>(RowCount);

        // Assert
        var softDeletes = await repository.SelectAllAsync<SoftDeleteWithoutIndexFilter>();
        softDeletes.Should()
            .HaveCount(RowCount)
            .And.AllSatisfy(e =>
            {
                e.IsDeleted.Should().BeTrue();
                e.DeletedAtUtc.Should().NotBeNull();
            });
    }
}
