namespace BymseRead.DataLayer.Repository;

public interface IBookmarksRepository
{
    void SetLastViewedPage(int bookId, int currentPage);
}