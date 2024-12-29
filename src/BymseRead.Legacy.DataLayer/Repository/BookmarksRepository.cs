using BymseRead.Legacy.DataLayer.Database;
using BymseRead.Legacy.DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BymseRead.Legacy.DataLayer.Repository;

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
                CreatedDate = DateTime.UtcNow
            };

            booksDbContext.Bookmarks.Add(bookmark);
        }
        else
        {
            bookmark.PageNumber = page;
            bookmark.CreatedDate = DateTime.UtcNow;
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

    public Bookmark FindBookmark(int bookmarkId)
    {
        return booksDbContext.Bookmarks.First(e => e.BookmarkId == bookmarkId);
    }

    public void SaveChanges(Bookmark bookmark)
    {
        if (bookmark.BookmarkId == 0)
        {
            booksDbContext.Bookmarks.Add(bookmark);
        }

        booksDbContext.SaveChanges();
    }

    public void DeleteBookmark(int bookmarkId)
    {
        booksDbContext.Bookmarks
            .Where(e => e.BookmarkId == bookmarkId)
            .ExecuteDelete();
    }
}