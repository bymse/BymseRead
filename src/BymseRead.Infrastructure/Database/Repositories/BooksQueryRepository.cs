using System.Data;
using System.Reflection;
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
        var cte = GetNumberedBookmarksCte();
        var sql = cte + """
                        select b.*, 
                               bp.*, 
                               bm.id, bm.book_id, bm.user_id, bm.type, bm.page, bm.created_at,
                               f.* from books as b
                        left join books_progress as bp on b.id = bp.book_id and bp.user_id = @userId
                        left join numbered_bookmarks as bm on b.id = bm.book_id
                        left join files as f on b.book_cover_file_id = f.id
                        where b.owner_user_id = @userId and bm.row_number = 1 {0}
                        order by b.id
                        """;

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = $"%{search.Replace("%", "[%]").Replace("_", "[_]")}%";
            sql = string.Format(sql, " and b.title like @search");
        }
        else
        {
            sql = string.Format(sql, "");
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

    private static string GetNumberedBookmarksCte()
    {
        const string sql = """
                           with numbered_bookmarks as (
                               select b.*, ROW_NUMBER() over (partition by b.book_id order by b.created_at desc) as row_number
                               from bookmarks as b
                               where b.user_id = @userId
                           )
                           """;

        return sql;
    }
}