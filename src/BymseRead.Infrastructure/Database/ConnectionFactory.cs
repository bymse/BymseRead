using System.Data.Common;
using Npgsql;

namespace BymseRead.Infrastructure.Database;

internal class ConnectionFactory(DataSourceProvider dataSourceProvider) : IDisposable
{
    private NpgsqlConnection? connection;

    public async ValueTask<DbConnection> Get()
    {
        return connection ??= await dataSourceProvider.Get().OpenConnectionAsync();
    }

    void IDisposable.Dispose()
    {
        connection?.Dispose();
    }
}