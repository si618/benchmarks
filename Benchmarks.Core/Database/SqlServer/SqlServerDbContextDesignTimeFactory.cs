namespace Benchmarks.Core.Database.SqlServer;

/// <summary>Create design time db context to enable ef migrations</summary>
public class SqlServerDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SqlServerDbContext>
{
    public SqlServerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SqlServerDbContext>();
        optionsBuilder.UseSqlServer(
        "Server=localhost;Database=Benchmarks;User Id=sa;Password=P@ssw0rd!;TrustServerCertificate=True;");

        return new SqlServerDbContext(optionsBuilder.Options);
    }
}
