using BymseBooks.DataLayer.Entity;

namespace BymseBooks.Core.Models;

public static class BookModelMapper
{
    public static BookExModel ToExModel(Book book)
    {
        return new BookExModel
        {
            Book = ToModel(book),
            Url = book.Url,
            Bookmarks = book.Bookmarks
                .OrderBy(e => e.PageNumber)
                .Select(e => new BookmarkModel
                {
                    Id = e.BookmarkId,
                    Type = e.BookmarkType,
                    Title = e.Title,
                    Date = e.CreatedDate,
                    Page = e.PageNumber
                }).ToArray()
        };
    }

    public static BookModel ToModel(Book b)
    {
        var lastPage = b.Bookmarks
            .LastOrDefault(r => r.BookmarkType == BookmarkType.LastPage)?
            .PageNumber;
        return new BookModel
        {
            Id = b.BookId,
            Title = b.Title,
            Author = b.AuthorName,
            State = b.State,
            Percents = GetPercents(lastPage, b.TotalPages, b.State),
            Tags = b.BookTags.Select(e => e.Tag.Title).ToArray(),
            TotalPages = b.TotalPages,
        };
    }

    private static int? GetPercents(int? lastPage, int? totalPages, BookState state)
    {
        if (state == BookState.New)
        {
            return 0;
        }

        if (state == BookState.Finished)
        {
            return 100;
        }

        if (!totalPages.HasValue)
        {
            return null;
        }

        if (!lastPage.HasValue)
        {
            return 0;
        }

        return (int)(Math.Round(((double)lastPage.Value) / totalPages.Value, 2) * 100);
    }

    public static void ToBook(Book book, BookExModel exModel, Tag[] tags)
    {
        var model = exModel.Book;
        book.Title = model.Title;
        book.AuthorName = model.Author;
        book.Url = exModel.Url;
        book.BookTags = tags.Select(e => new BookTagLink
        {
            Tag = e
        }).ToList();
        book.State = model.State;
    }
}