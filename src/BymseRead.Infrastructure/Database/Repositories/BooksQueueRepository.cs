using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using Dapper;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class BooksQueueRepository(ConnectionFactory connectionFactory)
{
    public async Task<BooksQueueItem?> GetNextItemForUpdate()
    {
        var connection = await connectionFactory.Get();
        var sql = $"""
                   SELECT id, book_id, status, created_at, updated_at
                   FROM books_queue
                   WHERE status = {(int) BookQueueItemStatus.Pending}
                   ORDER BY created_at
                   LIMIT 1
                   FOR UPDATE SKIP LOCKED
                   """;
        
        return await connection.QueryFirstOrDefaultAsync<BooksQueueItem>(sql);
    }

    public async Task Update(BooksQueueItem item)
    {
        var connection = await connectionFactory.Get();
        const string sql = """
                           UPDATE books_queue
                           SET status = @Status,
                           updated_at = @UpdatedAt
                           WHERE id = @Id
                           """;

        var updatedRows = await connection.ExecuteAsync(sql, item);
        if (updatedRows == 0)
        {
            throw new Exception("Failed to update book queue item");
        }
    }

    public async Task Add(BooksQueueItem booksQueueItem)
    {
        var connection = await connectionFactory.Get();
        const string sql = """
                           INSERT INTO books_queue (id, book_id, status, created_at, updated_at)
                           VALUES (@Id, @BookId, @Status, @CreatedAt, @UpdatedAt)
                           """;

        var insertedRows = await connection.ExecuteAsync(sql, booksQueueItem);
        if (insertedRows == 0)
        {
            throw new Exception("Failed to insert book queue item");
        }
    }
}