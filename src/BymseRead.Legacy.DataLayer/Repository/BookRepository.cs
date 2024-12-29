using System.Transactions;
using BymseRead.Legacy.DataLayer.Helpers;
using BymseRead.Legacy.DataLayer.Database;
using BymseRead.Legacy.DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BymseRead.Legacy.DataLayer.Repository
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

        public void DeleteBook(int bookId)
        {
            var book = context.Books.Find(bookId);
            if (book != null)
            {
                context.Books.Remove(book);
                context.SaveChanges();
            }
        }
    }
}