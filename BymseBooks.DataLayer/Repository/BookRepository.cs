using System.Transactions;
using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Entity;
using BymseBooks.DataLayer.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BymseBooks.DataLayer.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BooksDbContext context;

        public BookRepository(BooksDbContext context)
        {
            this.context = context;
        }

        public IReadOnlyList<Book> GetBooks(BookState state, int? takeCount, int? skipCount)
        {
            return context.Books
                .Include(e => e.Bookmarks)
                .Include(e => e.BookTags)
                .ThenInclude(e => e.Tag)
                .Where(e => e.State == state)
                .If(skipCount.HasValue, e => e.Skip(skipCount!.Value))
                .If(takeCount.HasValue, e => e.Take(takeCount!.Value))
                .ToArray();
        }


        public Book? FindBook(int bookId)
        {
            return context.Books
                .Include(e => e.Bookmarks)
                .Include(e => e.BookTags)
                .ThenInclude(e => e.Tag)
                .FirstOrDefault(e => e.BookId == bookId);
        }

        public void Delete(int bookId) => context.Books.Where(e => e.BookId == bookId).ExecuteDelete();

        public void UpdateTotalPages(int bookId, int totalPages)
        {
            context.Books
                .Where(e => e.BookId == bookId)
                .ExecuteUpdate(
                    e => e.SetProperty(r => r.TotalPages, totalPages)
                );
        }

        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
            {
                context.Books.Add(book);
            }

            context.SaveChanges();
        }
    }
}