using BymseRead.Legacy.DataLayer.Entity;

namespace BymseRead.Legacy.Core.Models;

public static class BookModelMapper
{
    public static BookModel ToModel(Book b)
    {
        var lastViewedPage = GetLastViewedPage(b);
        return new BookModel
        {
            Id = b.BookId,
            Title = b.Title,
            Author = b.AuthorName,
            State = b.State,
            Tags = b.BookTags.Select(e => e.Tag.Title).ToArray(),
            TotalPages = b.TotalPages,
            LastViewedPage = lastViewedPage,
            Url = b.Url,
            PercentageRead = b.GetPercentageRead(lastViewedPage) 
        };
    }

    private static int? GetLastViewedPage(Book book)
    {
        return book
            .Bookmarks
            .Where(e => e.BookmarkType == BookmarkType.LastViewedPage)
            .MaxBy(e => e.PageNumber)?
            .PageNumber;
    }

    public static void ToBook(Book book, BookModel model, Tag[] tags)
    {
        book.Title = model.Title;
        book.AuthorName = model.Author;
        book.Url = model.Url;
        book.BookTags = tags.Select(e => new BookTagLink
        {
            Tag = e
        }).ToList();
        book.State = model.State;
    }
}