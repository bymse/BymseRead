using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using Dapper;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class BooksRepository(ConnectionFactory connectionFactory) : IBooksRepository
{
    public async Task Add(Book book)
    {
        var connection = await connectionFactory.Get();
        const string sql = """
                            insert into books (
                                               id,
                                               title,
                                               created_at,
                                               pages,
                                               book_file_id,
                                               book_cover_file_id,
                                               owner_user_id
                                               )
                           values (@Id, @Title, @CreatedAt, @Pages, @BookFileId, @BookCoverFileId, @OwnerUserId);
                           """;

        var insertedRows = await connection.ExecuteAsync(sql, book);
        if (insertedRows != 1)
        {
            throw new InvalidOperationException("Failed to insert book");
        }
    }

    public async Task Update(Book book)
    {
        var connection = await connectionFactory.Get();
        const string sql = """
                            update books set
                                title = @Title,
                                book_file_id = @BookFileId,
                                book_cover_file_id = @BookCoverFileId
                            where id = @Id;
                           """;

        var updatedRows = await connection.ExecuteAsync(sql, book);
        if (updatedRows != 1)
        {
            throw new InvalidOperationException("Failed to update book");
        }
    }
}