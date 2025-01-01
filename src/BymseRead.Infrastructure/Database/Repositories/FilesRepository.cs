using BymseRead.Core.Common;
using BymseRead.Core.Repositories;
using Dapper;
using File = BymseRead.Core.Entities.File;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class FilesRepository(ConnectionFactory connectionFactory) : IFilesRepository
{
    public async Task Add(File file)
    {
        var connection = await connectionFactory.Get();
        const string sql = """
                            insert into files (id, name, path, "size", created_at)
                            values (@Id, @Name, @Path, @Size, @CreatedAt);
                           """;

        var insertedRows = await connection.ExecuteAsync(sql, file);
        if (insertedRows != 1)
        {
            throw new InvalidOperationException("Failed to insert file");
        }
    }
}