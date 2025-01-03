using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using Dapper;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class BookProgressRepository(ConnectionFactory connectionFactory) : IBookProgressRepository
{
    public async Task Upsert(BookProgress bookProgress)
    {
        var connection = await connectionFactory.Get();
        const string sql = """
                  INSERT INTO books_progress (id, book_id, user_id, started_at, current_page_change_at, current_page)
                  VALUES (@Id, @BookId, @UserId, @StartedAt, @CurrentPageChangeAt, @CurrentPage)
                  ON CONFLICT (book_id, user_id) DO UPDATE SET
                      current_page_change_at = @CurrentPageChangeAt,
                      current_page = @CurrentPage
                  """;
        
        var rows = await connection.ExecuteAsync(sql, bookProgress);
        if (rows == 0)
        {
            throw new Exception("Failed to upsert book progress");
        }
    }
}