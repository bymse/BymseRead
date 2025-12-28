using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using Dapper;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class BooksQueueRepository(ConnectionFactory connectionFactory)
{
    public async Task<BooksQueueItem[]> GetNextItemsForUpdate()
    {
        var connection = await connectionFactory.Get();
        var sql = $"""
                   with pending_books as (SELECT book_id, min(created_at) as earliest
                                          FROM books_queue
                                          WHERE status = {(int)BookQueueItemStatus.Pending}
                                          group by book_id
                                          order by earliest
                                          limit 1)
                   SELECT id, book_id, status, created_at, updated_at
                   FROM books_queue
                   WHERE book_id in (select book_id from pending_books)
                   ORDER BY created_at
                   FOR UPDATE SKIP LOCKED
                   """;

        var results = await connection.QueryAsync<BooksQueueItem>(sql);
        return results.ToArray();
    }

    public async Task Update(BooksQueueItemId[] items, BookQueueItemStatus status, DateTimeOffset updatedAt)
    {
        var connection = await connectionFactory.Get();
        const string sql = """
                           UPDATE books_queue
                           SET status = @Status,
                           updated_at = @UpdatedAt
                           WHERE id = ANY(@Ids)
                           """;

        var updatedRows = await connection.ExecuteAsync(sql,
            new
            {
                Ids = items,
                Status = status,
                UpdatedAt = updatedAt,
            });

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