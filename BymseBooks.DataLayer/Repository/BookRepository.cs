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

        public IReadOnlyList<Book> GetBooks(int userId, out int booksCount,
            int? takeCount = null, int? skipCount = null,
            IList<int>? tagsIds = null)
        {
            // using var transaction = new TransactionScope();
            //
            // IQueryable<Book> query;
            //
            // if (!tagsIds.IsNullOrEmpty())
            //
            // {
            //     var booksIds = context.BookTagLinks
            //         .Where(link => tagsIds!.Contains(link.TagId))
            //         .GroupBy(r => r.BookId)
            //         .Where(g => g.Count() == tagsIds!.Count)
            //         .Select(r => r.Key);
            //
            //     query = GetUserBooks(userId).Where(r => booksIds.Contains(r.BookId));
            // }
            // else
            // {
            //     query = GetUserBooks(userId);
            // }
            //
            // booksCount = query.Count();
            //
            // var models = query
            //     .OrderBy(e => e.State == BookState.Active
            //         ? 0
            //         : e.State + 2)
            //     .If(skipCount.HasValue, e => e.Skip(skipCount!.Value))
            //     .If(takeCount.HasValue, e => e.Take(takeCount!.Value))
            //     .OrderBy(e => e.State == BookState.Active
            //         ? 0
            //         : e.State + 2)
            //     .Select(book => new BookModel
            //     {
            //         BookId = book.BookId,
            //         AuthorName = book.AuthorName,
            //         Title = book.Title,
            //         TagsIds = book.BookTags.Select(tag => tag.TagId).ToList(),
            //         Url = book.Url,
            //         State = book.State,
            //     })
            //     .ToArray();
            //
            // transaction.Complete();
            // return models;
            throw new NotImplementedException();
        }

        public IReadOnlyList<Book> GetBooks(BookState state, int takeCount, int skipCount)
        {
            return context.Books
                .Include(e => e.BookTags)
                .ThenInclude(e => e.Tag)
                .Where(e => e.State == state)
                .Skip(skipCount)
                .Take(takeCount)
                .ToArray();
        }


        public bool Exist(string title, string authorName) =>
            context.Books.Any(e => e.Title == title && e.AuthorName == authorName);

        public bool Exist(int bookId, int userId)
        {
            return context.Books.Any(e => e.BookId == bookId);
        }

        public Book? FindBook(int bookId, int userId)
        {
            return context.Books.FirstOrDefault(e => e.BookId == bookId);
        }

        public int Insert(Book book)
        {
            context.Books.Add(book);
            context.SaveChanges();
            return book.BookId;
        }

        public void Update(int bookId, string? title, string? author, string? url, IList<int>? tags)
        {
            var book = context.Books.FirstOrDefault(e => e.BookId == bookId) ?? new Book();

            book.Title = title!.FallbackWith(book.Title);
            book.AuthorName = author!.FallbackWith(book.AuthorName);
            book.Url = url!.FallbackWith(book.Url!);

            if (!tags.IsNullOrEmpty())
            {
                context.BookTagLinks.Where(e => e.BookId == bookId).ExecuteDelete();
                book.BookTags = tags!.Select(e => new BookTagLink
                {
                    BookId = bookId,
                    TagId = e
                }).ToArray();
            }

            context.SaveChanges();
        }

        public void Update(int bookId, BookState state)
        {
            context.Books
                .Where(r => r.BookId == bookId)
                .ExecuteUpdate(
                    r => r.SetProperty(e => e.State, state)
                );
        }

        public void Delete(int bookId) => context.Books.Where(e => e.BookId == bookId).ExecuteDelete();
    }
}