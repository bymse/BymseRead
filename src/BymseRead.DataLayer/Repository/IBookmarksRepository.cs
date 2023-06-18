namespace BymseRead.DataLayer.Repository;

public interface IBookmarksRepository
{
    void SetLastPage(int bookId, int currentPage);
}