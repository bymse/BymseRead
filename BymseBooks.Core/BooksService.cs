using BymseBooks.Core.Models;
using BymseBooks.DataLayer.Entity;
using BymseBooks.DataLayer.Repository;

namespace BymseBooks.Core;

public class BooksService
{
    private readonly IBookRepository bookRepository;
    private readonly IBookmarksRepository bookmarksRepository;
    private readonly ITagsRepository tagsRepository;

    public BooksService(
        IBookRepository bookRepository,
        IBookmarksRepository bookmarksRepository,
        ITagsRepository tagsRepository
    )
    {
        this.bookRepository = bookRepository;
        this.bookmarksRepository = bookmarksRepository;
        this.tagsRepository = tagsRepository;
    }

    public BookModel[] GetBooks(BookState state, int? takeCount = null, int? skipCount = null)
    {
        var books = bookRepository.GetBooks(state, takeCount, skipCount);
        return books.Select(BookModelMapper.ToModel).ToArray();
    }

    public BookExModel? FindBook(int bookId)
    {
        var book = bookRepository.FindBook(bookId);
        if (book == null)
        {
            return null;
        }

        return BookModelMapper.ToExModel(book);
    }

    public BookExModel UpdateBook(BookExModel bookExModel)
    {
        var model = bookExModel.Book;
        var book = bookRepository.FindBook(model.Id)!;
        book.Title = model.Title;
        book.AuthorName = model.Author;
        book.Url = bookExModel.Url;
        book.BookTags = GetTags(bookExModel.Book.Tags)
            .Select(e => new BookTagLink
            {
                Tag = e
            }).ToList();

        bookRepository.SaveChanges();
        return BookModelMapper.ToExModel(book);
    }

    public void UpdateTotalPages(int bookId, int totalPages) => bookRepository.UpdateTotalPages(bookId, totalPages);
    public void UpdateLastPage(int bookId, int currentPage) => bookmarksRepository.SetLastPage(bookId, currentPage);

    private Tag[] GetTags(IList<string> tags)
    {
        var existingTags = tagsRepository.FindTagsByName(tags);
        var tagsToCreate = tags.Except(existingTags.Select(e => e.Title)).ToArray();
        var createdTags = tagsRepository.CreateTags(tagsToCreate);

        return existingTags.Concat(createdTags).ToArray();
    }
}