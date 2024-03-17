namespace Benchmarks.Core.Database.Postgres;

/// <summary>Create design time db context to enable ef migrations.</summary>
public class PostgresDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PostgresDbContext>
{
    public PostgresDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=Benchmarks;Username=user;Password=P@ssw0rd!");

        return new PostgresDbContext(optionsBuilder.Options);
    }
}
