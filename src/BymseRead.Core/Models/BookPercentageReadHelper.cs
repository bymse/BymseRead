using BymseRead.DataLayer.Entity;

namespace BymseRead.Core.Models;

public static class BookPercentageReadHelper
{
    public static int? GetPercentageRead(this Book book, int? lastViewedPage)
    {
        if (book.State == BookState.New)
        {
            return 0;
        }

        if (book.State == BookState.Finished)
        {
            return 100;
        }

        if (!book.TotalPages.HasValue)
        {
            return null;
        }

        if (!lastViewedPage.HasValue)
        {
            return 0;
        }

        return (int)(Math.Round(((double)lastViewedPage.Value) / book.TotalPages.Value, 2) * 100);
    }
}