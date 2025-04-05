using System.Data.Common;
using Npgsql;

namespace BymseRead.Infrastructure.Database;

internal class ConnectionFactory(DataSourceProvider dataSourceProvider) : IDisposable
{
    private NpgsqlConnection? _connection;

    public async ValueTask<DbConnection> Get()
    {
        return _connection ??= await dataSourceProvider.Get().OpenConnectionAsync();
    }

    void IDisposable.Dispose()
    {
        _connection?.Dispose();
    }
}