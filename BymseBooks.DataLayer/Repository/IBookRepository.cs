using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Repository
{
    public interface IBookRepository
    {
        IReadOnlyList<Book> GetBooks(BookState state, int? takeCount, int? skipCount);

        Book? FindBook(int bookId);
        void UpdateTotalPages(int bookId, int totalPages);

        void SaveChanges();
    }
}