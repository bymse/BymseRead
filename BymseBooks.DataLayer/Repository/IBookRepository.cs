using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Repository
{
    public interface IBookRepository
    {
        IReadOnlyList<Book> GetBooks(int userId,
            out int booksCount,
            int? takeCount = null, 
            int? skipCount = null, 
            IList<int>? tagsIds = null);

        IReadOnlyList<Book> GetBooks(BookState state, int takeCount, int skipCount);

        Book? FindBook(int bookId, int userId);
        
        bool Exist(string title, string authorName);
        bool Exist(int bookId, int userId);
        
        int Insert(Book book);
        void Update(int bookId, string? title, string? author, string? url, IList<int>? tags);
        void Update(int bookId, BookState state);
        void Delete(int bookId);
    }
}