using BymseBooks.Core.Models;
using BymseBooks.DataLayer.Entity;
using BymseBooks.DataLayer.Repository;

namespace BymseBooks.Core;

public class BooksService
{
    private readonly IBookRepository bookRepository;
    private readonly IBookmarksRepository bookmarksRepository;

    public BooksService(IBookRepository bookRepository, IBookmarksRepository bookmarksRepository)
    {
        this.bookRepository = bookRepository;
        this.bookmarksRepository = bookmarksRepository;
    }

    public BookModel[] GetBooks(BookState state, int? takeCount = null, int? skipCount = null)
    {
        var books = bookRepository.GetBooks(state, takeCount, skipCount);
        return books.Select(ToModel).ToArray();
    }

    public BookExModel? FindBook(int bookId)
    {
        var book = bookRepository.FindBook(bookId);
        if (book == null)
        {
            return null;
        }

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

    public void UpdateTotalPages(int bookId, int totalPages) => bookRepository.UpdateTotalPages(bookId, totalPages);
    public void UpdateLastPage(int bookId, int currentPage) => bookmarksRepository.SetLastPage(bookId, currentPage);

    private static BookModel ToModel(Book b)
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
}