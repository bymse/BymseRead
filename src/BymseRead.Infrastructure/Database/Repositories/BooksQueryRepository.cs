using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using Dapper;
using File = BymseRead.Core.Entities.File;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class BooksQueryRepository(ConnectionFactory connectionFactory) : IBooksQueryRepository
{
    public async Task<IEnumerable<UserBookModel>> GetUserBooks(UserId userId, string? search)
    {
        var connection = await connectionFactory.Get();

        var sql = """
                  select * from books as b
                  left join books_progress as bp on b.id = bp.book_id and bp.user_id = @userId
                  left join bookmarks as bm on b.id = bm.book_id and bm.user_id = @userId
                  left join files as f on b.book_cover_file_id = f.id
                  order by b.id
                  """;

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = $"%{search?.Replace("%", "[%]").Replace("_", "[_]")}%";
            sql += " where books.title like @search";
        }

        return await connection.QueryAsync<Book, BookProgress, Bookmark, File, UserBookModel>(sql,
            (book, progress, bookmark, file) => new UserBookModel
            {
                Book = book,
                Progress = progress,
                LastBookmark = bookmark,
                CoverFile = file,
            },
            new { userId = userId.Value, search });
    }
}