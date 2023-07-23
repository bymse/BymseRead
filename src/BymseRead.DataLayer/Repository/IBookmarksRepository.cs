using BymseRead.DataLayer.Entity;

namespace BymseRead.DataLayer.Repository;

public interface IBookmarksRepository
{
    void SetPageForLastBookmark(int bookId, BookmarkType type, int page);
}