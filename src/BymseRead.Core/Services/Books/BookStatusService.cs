using BymseRead.Core.Entities;

namespace BymseRead.Core.Services.Books;

public static class BookStatusService
{
    public static BookStatus Get(Book book, BookProgress? bookProgress, Bookmark? bookmark)
    {
        var bookmarkPage = bookmark?.Page ?? 0;
        var bookProgressPage = bookProgress?.CurrentPage ?? 0;
        var lastPage = Math.Max(bookmarkPage, bookProgressPage);

        if (lastPage < 3)
        {
            return BookStatus.New;
        }

        if (book.Pages.HasValue && IsLessThen3PagesToEnd(lastPage, book.Pages.Value))
        {
            return BookStatus.Archived;
        }

        if ((bookmark?.CreatedAt != null && IsMoreThan4Weeks(bookmark.CreatedAt)) ||
            (bookProgress?.CurrentPageChangeAt != null && IsMoreThan4Weeks(bookProgress.CurrentPageChangeAt.Value)))
        {
            return BookStatus.TlDr;
        }

        return BookStatus.Active;
    }

    private static bool IsLessThen3PagesToEnd(int lastPage, int totalPages)
    {
        return totalPages > 3 && totalPages - lastPage < 3;
    }

    private static bool IsMoreThan4Weeks(DateTimeOffset lastRead)
    {
        return DateTimeOffset.UtcNow - lastRead > TimeSpan.FromDays(28);
    }
}

public enum BookStatus
{
    Active,
    New,
    TlDr,
    Archived,
}