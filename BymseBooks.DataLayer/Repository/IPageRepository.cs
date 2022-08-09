using BymseBooks.DataLayer.Entity;
using BymseBooks.DataLayer.Models;

namespace BymseBooks.DataLayer.Repository
{
    public interface IPageRepository
    {
        IReadOnlyList<PageModel> GetPagesAndFetchTotalCount(int bookId, 
            int? count, int? skipCount, 
            PagesOrder order, bool orderByDescending, 
            out int totalCount);
        bool Exist(int pageId, int userId);
        int InsertPageAndGetId(Page page);
        void UpdatePage(int pageId, string pageNumber, DateTime? date, DateTime lastModified);
        void DeletePage(int pageId);

        void InsertBatch(IReadOnlyList<Page> pages);
    }
}