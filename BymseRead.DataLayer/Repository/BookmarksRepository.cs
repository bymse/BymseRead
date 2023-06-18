using BymseRead.DataLayer.Database;
using BymseRead.DataLayer.Entity;

namespace BymseRead.DataLayer.Repository;

public class BookmarksRepository : IBookmarksRepository
{
    private readonly BooksDbContext booksDbContext;

    public BookmarksRepository(BooksDbContext booksDbContext)
    {
        this.booksDbContext = booksDbContext;
    }

    public void SetLastPage(int bookId, int currentPage)
    {
        var bookmark = booksDbContext.Bookmarks
            .Where(e => e.BookmarkType == BookmarkType.LastPage)
            .OrderBy(e => e.CreatedDate)
            .LastOrDefault(e => e.BookId == bookId);

        if (bookmark == null)
        {
            bookmark = new Bookmark
            {
                BookId = bookId,
                BookmarkType = BookmarkType.LastPage,
                PageNumber = currentPage,
                CreatedDate = DateTime.Now
            };

            booksDbContext.Bookmarks.Add(bookmark);
        }
        else
        {
            bookmark.PageNumber = currentPage;
        }

        booksDbContext.SaveChanges();
    }
}