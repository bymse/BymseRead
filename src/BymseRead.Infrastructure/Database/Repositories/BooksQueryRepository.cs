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
        return await LoadUserBooks(userId, null, search);
    }

    public async Task<UserBookModel?> FindUserBook(UserId userId, BookId bookId)
    {
        var books = await LoadUserBooks(userId, bookId, null);
        return books.SingleOrDefault();
    }

    public async Task<BookModel?> FindBook(UserId userId, BookId bookId)
    {
        const string sql = """
                           select b.*, cf.*, bf.*
                           from books as b
                                    left join files as cf on b.book_cover_file_id = cf.id
                                    left join files as bf on b.book_file_id = bf.id
                           where b.owner_user_id = @userId and b.id = @bookId
                           """;

        var connection = await connectionFactory.Get();

        var result = await connection.QueryAsync<Book, File, File, BookModel>(sql,
            (book, coverFile, bookFile) => new BookModel
            {
                Book = book, CoverFile = coverFile, BookFile = bookFile,
            },
            new { userId, bookId });

        return result.SingleOrDefault();
    }

    private async Task<IEnumerable<UserBookModel>> LoadUserBooks(UserId userId, BookId? bookId, string? search)
    {
        if (bookId != null && search != null)
        {
            throw new ArgumentException("Cannot search by bookId and search at the same time");
        }

        var sql = """
                  with numbered_bookmarks as (
                  select b.*, ROW_NUMBER() over (partition by b.book_id order by b.created_at desc) as row_number
                      from bookmarks as b
                      where b.user_id = @userId
                  )
                  select b.*, 
                         bp.*, 
                         bm.id, bm.book_id, bm.user_id, bm.type, bm.page, bm.created_at,
                         cf.*,
                         bf.*
                  from books as b
                  left join books_progress as bp on b.id = bp.book_id and bp.user_id = @userId
                  left join numbered_bookmarks as bm on b.id = bm.book_id
                  left join files as cf on b.book_cover_file_id = cf.id
                  left join files as bf on b.book_file_id = bf.id
                  where b.owner_user_id = @userId and COALESCE(bm.row_number, 1) = 1 {0}
                  order by b.id
                  """;

        if (bookId != null)
        {
            sql = string.Format(sql, " and b.id = @bookId");
        }
        else if (search != null)
        {
            search = $"%{search.Replace("%", "[%]").Replace("_", "[_]")}%";
            sql = string.Format(sql, " and b.title like @search");
        }
        else
        {
            sql = string.Format(sql, "");
        }

        var connection = await connectionFactory.Get();
        return await connection.QueryAsync<Book, BookProgress, Bookmark, File, File, UserBookModel>(sql,
            (book, progress, bookmark, coverFile, bookFile) => new UserBookModel
            {
                Book = book,
                Progress = progress,
                LastBookmark = bookmark,
                CoverFile = coverFile,
                BookFile = bookFile,
            },
            new { userId, bookId, search });
    }
}