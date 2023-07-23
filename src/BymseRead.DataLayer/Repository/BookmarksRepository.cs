using BymseRead.DataLayer.Database;
using BymseRead.DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BymseRead.DataLayer.Repository;

public class BookmarksRepository : IBookmarksRepository
{
    private readonly BooksDbContext booksDbContext;

    public BookmarksRepository(BooksDbContext booksDbContext)
    {
        this.booksDbContext = booksDbContext;
    }

    public void SetPageForLastBookmark(int bookId, BookmarkType type, int page)
    {
        var bookmark = booksDbContext.Bookmarks
            .Where(e => e.BookId == bookId)
            .Where(e => e.BookmarkType == type)
            .OrderBy(e => e.CreatedDate)
            .LastOrDefault();

        if (bookmark == null)
        {
            bookmark = new Bookmark
            {
                BookId = bookId,
                BookmarkType = type,
                PageNumber = page,
                CreatedDate = DateTime.Now
            };

            booksDbContext.Bookmarks.Add(bookmark);
        }
        else
        {
            bookmark.PageNumber = page;
        }

        booksDbContext.SaveChanges();
    }

    public Bookmark[] GetBookmarks(int bookModelId)
    {
        return booksDbContext.Bookmarks
            .AsNoTracking()
            .Where(e => e.BookId == bookModelId)
            .OrderByDescending(e => e.PageNumber)
            .ToArray();
    }
}