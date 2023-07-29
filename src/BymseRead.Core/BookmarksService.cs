using BymseRead.Core.Models;
using BymseRead.DataLayer.Entity;
using BymseRead.DataLayer.Repository;

namespace BymseRead.Core;

public class BookmarksService
{
    private readonly IBookmarksRepository bookmarksRepository;

    public BookmarksService(IBookmarksRepository bookmarksRepository)
    {
        this.bookmarksRepository = bookmarksRepository;
    }

    public void SetLastViewedPage(int bookId, int page) =>
        bookmarksRepository.SetPageForLastBookmark(bookId, BookmarkType.LastViewedPage, page);

    public void SetLastPage(int bookId, int page) =>
        bookmarksRepository.SetPageForLastBookmark(bookId, BookmarkType.LastPage, page);

    public BookmarksListModel GetBookmarksList(BookModel bookModel)
    {
        var bookmarks = bookmarksRepository.GetBookmarks(bookModel.Id);

        return new BookmarksListModel
        {
            CustomBookmarks = bookmarks
                .Where(e => e.BookmarkType == BookmarkType.Custom)
                .Select(e => ToModel(e, ""))
                .ToArray(),
            LastPageBookmark = bookmarks
                .Where(e => e.BookmarkType == BookmarkType.LastPage)
                .Select(e => ToModel(e, "Last page"))
                .FirstOrDefault(),
        };
    }

    private static BookmarkModel ToModel(Bookmark bookmark, string titleFallback)
    {
        return new BookmarkModel
        {
            Id = bookmark.BookmarkId,
            Type = bookmark.BookmarkType,
            Title = bookmark.Title ?? titleFallback,
            Date = bookmark.CreatedDate,
            Page = bookmark.PageNumber,
            ColorCode = bookmark.ColorCode,
        };
    }
}