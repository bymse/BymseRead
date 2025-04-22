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

        if ((bookmark?.CreatedAt != null && IsMoreThen2Weeks(bookmark.CreatedAt)) ||
            (bookProgress?.CurrentPageChangeAt != null && IsMoreThen2Weeks(bookProgress.CurrentPageChangeAt.Value)))
        {
            return BookStatus.TlDr;
        }

        return BookStatus.Active;
    }

    private static bool IsLessThen3PagesToEnd(int lastPage, int totalPages)
    {
        return totalPages > 3 && totalPages - lastPage < 3;
    }

    private static bool IsMoreThen2Weeks(DateTimeOffset lastRead)
    {
        return DateTimeOffset.UtcNow - lastRead > TimeSpan.FromDays(14);
    }
}

public enum BookStatus
{
    Active,
    New,
    TlDr,
    Archived,
}