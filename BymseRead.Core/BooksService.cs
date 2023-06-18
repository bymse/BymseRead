using BymseRead.Core.Models;
using BymseRead.DataLayer.Entity;
using BymseRead.DataLayer.Repository;

namespace BymseRead.Core;

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

    public BookExModel SaveBook(BookExModel bookExModel)
    {
        var book = bookExModel.Book.Id != 0
            ? bookRepository.FindBook(bookExModel.Book.Id)!
            : new Book
            {
                Bookmarks = new List<Bookmark>(),
                CreatedDate = DateTime.UtcNow
            };
        BookModelMapper.ToBook(book, bookExModel, GetTags(bookExModel.Book.Tags));
        bookRepository.SaveBook(book);

        return BookModelMapper.ToExModel(book);
    }

    public void DeleteBook(BookExModel bookExModel)
    {
        bookRepository.DeleteBook(bookExModel.Book.Id);
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