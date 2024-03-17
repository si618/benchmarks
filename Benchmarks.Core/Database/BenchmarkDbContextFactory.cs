namespace Benchmarks.Core.Database;

public static class BenchmarkDbContextFactory
{
    public static BenchmarkDbContext Create(DbServer server, string connectionString)
    {
        switch (server)
        {
            case DbServer.Postgres:
                var postgresOptions = new DbContextOptionsBuilder<PostgresDbContext>();
                postgresOptions.UseNpgsql(connectionString);
                return new PostgresDbContext(postgresOptions.Options);

            case DbServer.SqlServer:
                var sqlServerOptions = new DbContextOptionsBuilder<SqlServerDbContext>();
                sqlServerOptions.UseSqlServer(connectionString);
                return new SqlServerDbContext(sqlServerOptions.Options);

            default:
                throw new NotImplementedException();
        }
    }
}
