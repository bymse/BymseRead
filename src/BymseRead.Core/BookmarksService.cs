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

    public void SaveBookmark(BookmarkModel model, int bookId)
    {
        var bookmark = model.Id != 0
            ? bookmarksRepository.FindBookmark(model.Id)!
            : new Bookmark();
        
        FromModel(model, bookmark);
        bookmark.BookId = bookId;
        bookmarksRepository.SaveChanges(bookmark);
    }
    
    
    private static BookmarkModel ToModel(Bookmark bookmark, string titleFallback)
    {
        return new BookmarkModel
        {
            Id = bookmark.BookmarkId,
            Type = bookmark.BookmarkType,
            Title = bookmark.Title ?? titleFallback,
            Date = bookmark.CreatedDate.ToLocalTime().Date,
            Page = bookmark.PageNumber,
            ColorCode = bookmark.ColorCode,
        };
    }

    private static void FromModel(BookmarkModel model, Bookmark bookmark)
    {
        bookmark.PageNumber = model.Page;
        bookmark.Title = model.Title;
        bookmark.CreatedDate = model.Date;
        bookmark.ColorCode = model.ColorCode;
        bookmark.BookmarkType = model.Type;
    }

    public void DeleteBookmark(BookmarkModel model)
    {
        bookmarksRepository.DeleteBookmark(model.Id);
    }
}