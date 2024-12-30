using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BymseRead.Infrastructure.Database;

internal class DataSourceProvider(IConfiguration configuration) : IDisposable
{
    private NpgsqlDataSource? dataSource;

    public NpgsqlDataSource Get()
    {
        return dataSource ??= Build();
    }

    private NpgsqlDataSource Build()
    {
        var connectionString = configuration.GetConnectionString("BymseReadPostgres") ??
                               throw new InvalidOperationException("Connection string not found");

        return new NpgsqlDataSourceBuilder(connectionString).Build();
    }

    void IDisposable.Dispose()
    {
        dataSource?.Dispose();
    }
}