using BymseRead.Core.Common;
using BymseRead.Infrastructure.Database.Entities;
using Dapper;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class DataProtectionKeysRepository(ConnectionFactory connectionFactory)
{
    public async Task<IReadOnlyCollection<DataProtectionKey>> GetAll()
    {
        var connection = await connectionFactory.Get();
        var sql = $"SELECT id, friendly_name, xml, created_at FROM {Tables.DataProtectionKeys}";

        var results = await connection.QueryAsync<DataProtectionKey>(sql);
        return [.. results];
    }

    public async Task Add(DataProtectionKey key)
    {
        var connection = await connectionFactory.Get();
        var sql = $"""
                   INSERT INTO {Tables.DataProtectionKeys} (id, friendly_name, xml, created_at)
                   VALUES (@Id, @FriendlyName, @Xml, @CreatedAt)
                   """;

        var insertedRows = await connection.ExecuteAsync(sql, key);
        if (insertedRows != 1)
        {
            throw new InvalidOperationException("Failed to insert data protection key");
        }
    }
}