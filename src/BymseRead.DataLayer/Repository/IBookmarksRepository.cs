using BymseRead.DataLayer.Entity;

namespace BymseRead.DataLayer.Repository;

public interface IBookmarksRepository
{
    void SetPageForLastBookmark(int bookId, BookmarkType type, int page);
    Bookmark[] GetBookmarks(int bookModelId);
    Bookmark FindBookmark(int bookmarkId);
    void SaveChanges(Bookmark bookmark);
    void DeleteBookmark(int bookmarkId);
}