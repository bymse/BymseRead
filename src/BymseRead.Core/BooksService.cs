using BymseRead.Core.Models;
using BymseRead.DataLayer.Entity;
using BymseRead.DataLayer.Repository;

namespace BymseRead.Core;

public class BooksService
{
    private readonly IBookRepository bookRepository;
    
    private readonly ITagsRepository tagsRepository;

    public BooksService(
        IBookRepository bookRepository,
        ITagsRepository tagsRepository
    )
    {
        this.bookRepository = bookRepository;
        this.tagsRepository = tagsRepository;
    }

    public BookModel[] GetBooks(BookState state, int? takeCount = null, int? skipCount = null)
    {
        var books = bookRepository.GetBooks(state, takeCount, skipCount);
        return books.Select(BookModelMapper.ToModel).ToArray();
    }

    public BookModel? FindBook(int bookId)
    {
        var book = bookRepository.FindBook(bookId);
        if (book == null)
        {
            return null;
        }

        return BookModelMapper.ToModel(book);
    }

    public BookModel SaveBook(BookModel bookModel)
    {
        var book = bookModel.Id != 0
            ? bookRepository.FindBook(bookModel.Id)!
            : new Book
            {
                Bookmarks = new List<Bookmark>(),
                CreatedDate = DateTime.UtcNow
            };
        BookModelMapper.ToBook(book, bookModel, GetTags(bookModel.Tags));
        bookRepository.SaveBook(book);

        return BookModelMapper.ToModel(book);
    }

    public void DeleteBook(BookModel bookExModel)
    {
        bookRepository.DeleteBook(bookExModel.Id);
    }

    public void UpdateTotalPages(int bookId, int totalPages) => bookRepository.UpdateTotalPages(bookId, totalPages);

    private Tag[] GetTags(IList<string> tags)
    {
        var existingTags = tagsRepository.FindTagsByName(tags);
        var tagsToCreate = tags.Except(existingTags.Select(e => e.Title)).ToArray();
        var createdTags = tagsRepository.CreateTags(tagsToCreate);

        return existingTags.Concat(createdTags).ToArray();
    }
}