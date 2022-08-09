using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Models.Extensions
{
    public static class BooksExtensions
    {
        public static IQueryable<BookModel> ToModels(this IQueryable<Book> books) => books
            .Select(book => new BookModel
            {
                BookId = book.BookId,
                AuthorName = book.AuthorName,
                Title = book.Title,
                TagsIds = book.BookTags.Select(tag => tag.TagId).ToList(),
                Url = book.Url,
                State = book.State,
            });
    }
}