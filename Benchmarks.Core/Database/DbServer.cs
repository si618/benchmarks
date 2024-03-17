namespace Benchmarks.Core.Database;

public enum DbServer
{
    Postgres,
    SqlServer
}

public static class DbServerExtensions
{
    public static InvalidEnumArgumentException InvalidEnumArgumentException(this DbServer dbServer)
        => new(nameof(dbServer), (int)dbServer, typeof(DbServer));
}
