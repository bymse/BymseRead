using System.Data.Common;
using Npgsql;

namespace BymseRead.Infrastructure.Database;

internal class ConnectionFactory(DataSourceProvider dataSourceProvider) : IDisposable
{
    private readonly List<NpgsqlConnection> connections = new();

    public async ValueTask<DbConnection> Get()
    {
        var connection = await dataSourceProvider.Get().OpenConnectionAsync();
        connections.Add(connection);

        return connection;
    }

    public void Dispose()
    {
        foreach (var connection in connections)
        {
            connection.Dispose();
        }
    }
}