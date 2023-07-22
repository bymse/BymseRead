using BymseRead.DataLayer.Repository;

namespace BymseRead.Core;

public class BookmarksService
{
    private readonly IBookmarksRepository bookmarksRepository;

    public BookmarksService(IBookmarksRepository bookmarksRepository)
    {
        this.bookmarksRepository = bookmarksRepository;
    }

    public void SetLastViewedPage(int bookId, int currentPage) => 
        bookmarksRepository.SetLastViewedPage(bookId, currentPage);
}