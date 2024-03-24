namespace Benchmarks.Tests.Repositories;

public class SoftDeleteRepositoryTests : RepositoryTestBase
{
    private const int RowCount = 1_000;

    private SoftDeleteRepository CreateRepository(DbServer dbServer) =>
        new(CreateDbContext(dbServer));

    [Theory]
    [InlineData(DbServer.Postgres)]
    [InlineData(DbServer.SqlServer)]
    public async Task CreateAsync_InsertsRows_ForExpectedRowCount(DbServer dbServer)
    {
        // Arrange
        var repository = CreateRepository(dbServer);
        await repository.MigrateAsync();

        // Act
        await repository.CreateAsync<HardDelete>(RowCount);
        await repository.CreateAsync<SoftDeleteWithIndexFilter>(RowCount);

        // Assert
        var hardDeletes = await repository.SelectAllAsync<HardDelete>();
        var softDeletes = await repository.SelectAllAsync<SoftDeleteWithIndexFilter>();
        hardDeletes.Should().HaveCount(RowCount);
        softDeletes.Should().HaveCount(RowCount);
    }

    [Theory]
    [InlineData(DbServer.Postgres)]
    [InlineData(DbServer.SqlServer)]
    public async Task DeleteAsync_DeletesRows_WhenUsingHardDelete(DbServer dbServer)
    {
        // Arrange
        var repository = CreateRepository(dbServer);
        await repository.MigrateAsync();
        await repository.CreateAsync<HardDelete>(RowCount);

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
        await repository.MigrateAsync();
        await repository.CreateAsync<SoftDeleteWithIndexFilter>(RowCount);

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
    public async Task SelectAllAsync_ReturnsAllRows_WithExpectedDeletedStatus(DbServer dbServer)
    {
        // Arrange
        const int delete = RowCount / 2;
        var repository = CreateRepository(dbServer);
        await repository.MigrateAsync();
        await repository.CreateAsync<SoftDeleteWithoutIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithoutIndexFilter>(delete);

        // Act
        var softDeletes = await repository.SelectAllAsync<SoftDeleteWithoutIndexFilter>();

        // Assert
        softDeletes.Should()
            .HaveCount(RowCount)
            .And.AllSatisfy(e =>
            {
                if (e.Id > delete)
                {
                    e.IsDeleted.Should().BeFalse();
                    e.DeletedAtUtc.Should().BeNull();
                }
                else
                {
                    e.IsDeleted.Should().BeTrue();
                    e.DeletedAtUtc.Should().NotBeNull();
                }
            });
    }

    [Theory]
    [InlineData(DbServer.Postgres)]
    [InlineData(DbServer.SqlServer)]
    public async Task SelectDeletedAsync_ReturnsOnlyDeletedRows(DbServer dbServer)
    {
        // Arrange
        const int deleteRowCount = RowCount / 3;
        var repository = CreateRepository(dbServer);
        await repository.MigrateAsync();
        await repository.CreateAsync<SoftDeleteWithoutIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithoutIndexFilter>(deleteRowCount);

        // Act
        var softDeletes = await repository.SelectDeletedAsync<SoftDeleteWithoutIndexFilter>();

        // Assert
        softDeletes.Should()
            .HaveCount(deleteRowCount)
            .And.AllSatisfy(e =>
            {
                e.IsDeleted.Should().BeTrue();
                e.DeletedAtUtc.Should().NotBeNull();
            });
    }

    [Theory]
    [InlineData(DbServer.Postgres)]
    [InlineData(DbServer.SqlServer)]
    public async Task SelectNonDeletedAsync_ReturnsOnlyNonDeletedRows(DbServer dbServer)
    {
        // Arrange
        const int deleteRowCount = RowCount / 3;
        var repository = CreateRepository(dbServer);
        await repository.MigrateAsync();
        await repository.CreateAsync<SoftDeleteWithoutIndexFilter>(RowCount);
        await repository.DeleteAsync<SoftDeleteWithoutIndexFilter>(deleteRowCount);

        // Act
        var softDeletes = await repository.SelectNonDeletedAsync<SoftDeleteWithoutIndexFilter>();

        // Assert
        softDeletes.Should()
            .HaveCount(RowCount - deleteRowCount)
            .And.AllSatisfy(e =>
            {
                e.IsDeleted.Should().BeFalse();
                e.DeletedAtUtc.Should().BeNull();
            });
    }
}
