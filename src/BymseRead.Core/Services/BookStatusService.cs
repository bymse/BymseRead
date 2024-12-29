using BymseRead.Core.Entities;

namespace BymseRead.Core.Services;

public static class BookStatusService
{
    public static BookStatus Get(Book book, BookProgress? bookProgress, Bookmark? bookmark)
    {
        if ((bookProgress == null && bookmark == null) || (bookProgress?.CurrentPage < 3 && bookmark == null))
        {
            return BookStatus.New;
        }

        var bookmarkPage = bookmark?.Page ?? 0;
        var bookProgressPage = bookProgress?.CurrentPage ?? 0;
        var lastPage = bookmarkPage > bookProgressPage ? bookmarkPage : bookProgressPage;

        if (IsLessThen3PagesToEnd(lastPage, book.Pages))
        {
            return BookStatus.Archived;
        }

        if (bookmark?.CreatedAt != null && IsMoreThen2Weeks(bookmark.CreatedAt))
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
    Archived
}