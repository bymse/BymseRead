using System.Data.Common;
using Npgsql;

namespace BymseRead.Infrastructure.Database;

internal class ConnectionFactory(DataSourceProvider dataSourceProvider) : IDisposable
{
    private readonly List<NpgsqlConnection> _connections = [];

    public async ValueTask<DbConnection> Get()
    {
        var connection = await dataSourceProvider.Get().OpenConnectionAsync();
        _connections.Add(connection);

        return connection;
    }

    void IDisposable.Dispose()
    {
        foreach (var connection in _connections)
        {
            connection.Dispose();
        }
    }
}