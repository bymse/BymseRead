using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using Dapper;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class BookmarksRepository(ConnectionFactory connectionFactory) : IBookmarksRepository
{
    public async Task Add(Bookmark bookmark)
    {
        var connection = await connectionFactory.Get();
        var rows = await connection.ExecuteAsync(
            "INSERT INTO bookmarks (id, book_id, user_id, type, page, created_at) VALUES (@Id, @BookId, @UserId, @Type, @Page, @CreatedAt)",
            bookmark);

        if (rows != 1)
        {
            throw new InvalidOperationException("Failed to insert bookmark");
        }
    }
}