using System.Linq.Expressions;
using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Models.Helpers
{
    public static class PageOrderHelper
    {
        public static Expression<Func<Page, object>> SwitchExpression(this PagesOrder order)
        {
            return order switch
            {
                PagesOrder.ByDate => e => e.Date,
                PagesOrder.ByPageNumber => e => e.Number,
                _ => throw new ArgumentException(nameof(order))
            };
        }
    }
}