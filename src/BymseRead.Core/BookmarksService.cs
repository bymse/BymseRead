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
}