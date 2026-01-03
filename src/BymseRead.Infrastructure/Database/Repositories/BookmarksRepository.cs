using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using Dapper;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class BookmarksRepository(ConnectionFactory connectionFactory) : IBookmarksRepository
{
    public async Task<int> Add(Bookmark bookmark)
    {
        var connection = await connectionFactory.Get();
        const string sql = """
                  INSERT INTO bookmarks (id, book_id, user_id, type, page, created_at)
                  SELECT @Id, @BookId, @UserId, @Type, @Page, @CreatedAt
                  WHERE @CreatedAt >
                        COALESCE(
                          (SELECT MAX(b.created_at)
                           FROM bookmarks b
                           WHERE b.book_id = @BookId
                             AND b.user_id = @UserId),
                          '-infinity'::timestamptz
                        )
                  """;

        return await connection.ExecuteAsync(sql, bookmark);
    }
}