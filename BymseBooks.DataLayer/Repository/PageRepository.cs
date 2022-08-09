using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Entity;
using BymseBooks.DataLayer.Helpers;
using BymseBooks.DataLayer.Models;
using BymseBooks.DataLayer.Models.Helpers;

namespace BymseBooks.DataLayer.Repository
{
    public class PageRepository : IPageRepository
    {
        private readonly BooksDbContext context;

        public PageRepository(BooksDbContext context)
        {
            this.context = context;
        }

        public IReadOnlyList<PageModel> GetPagesAndFetchTotalCount(int bookId,
            int? count, int? skipCount,
            PagesOrder order, bool orderByDescending,
            out int totalCount)
        {
            var query = context.Pages
                .Where(e => e.BookId == bookId);

            totalCount = query.Count();

            return query
                .OrderBy(order.SwitchExpression(), orderByDescending)
                .If(skipCount.HasValue, e => e.Skip(skipCount!.Value))
                .If(count.HasValue, e => e.Take(count!.Value))
                .Select(e => new PageModel
                {
                    Date = e.Date,
                    Number = e.Number,
                    PageId = e.PageId
                })
                .ToArray();
        }

        public bool Exist(int pageId, int userId)
        {
            return context.Pages
                .Where(e => e.PageId == pageId)
                .Any(e => e.Book.UserId == userId);
        }

        public int InsertPageAndGetId(Page page)
        {
            context.Pages.Add(page);
            context.SaveChanges();
            return page.PageId;
        }

        public void UpdatePage(int pageId, string pageNumber, DateTime? date, DateTime lastModified)
        {
            var page = new Page
            {
                PageId = pageId,
            };

            context.Pages.Attach(page);
            if (!pageNumber.IsNullOrEmpty())
                page.Number = pageNumber!;

            if (date.HasValue)
                page.Date = date.Value;

            page.LastModifiedDate = lastModified;

            context.SaveChanges();
        }

        public void DeletePage(int pageId)
        {
            context.Pages
                .Where(e => e.PageId == pageId)
                .DeleteFromQuery();
        }

        public void InsertBatch(IReadOnlyList<Page> pages)
        {
            context.Pages.AddRange(pages);
            context.SaveChanges();
        }
    }
}