using BymseBooks.App.ViewModel;
using BymseBooks.DataLayer.Entity;
using BymseBooks.DataLayer.Repository;

namespace BymseBooks.App.Service;

public class BooksService
{
    private readonly IBookRepository bookRepository;

    public BooksService(IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;
    }

    public IReadOnlyList<BookViewModel> GetBooks(BookState state, int takeCount, int skipCount)
    {
        return bookRepository.GetBooks(state, takeCount, skipCount)
            .Select(e =>
            {
                var lastPage = e.Bookmarks
                    .LastOrDefault(r => r.BookmarkType == BookmarkType.LastPage)?
                    .PageNumber;
                return new BookViewModel
                {
                    Author = e.AuthorName,
                    Title = e.Title,
                    BookId = e.BookId,
                    BookState = e.State,
                    Percents = GetPercents(lastPage, e.TotalPages)
                };
            })
            .ToArray();
    }

    private static int? GetPercents(int? lastPage, int totalPages)
    {
        if (!lastPage.HasValue)
        {
            return null;
        }
        
        return (int?)(Math.Round(((double) lastPage) / totalPages, 2) * 100);
    }
}