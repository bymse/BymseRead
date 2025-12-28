using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using BymseRead.Core.Services.Books;
using BymseRead.Core.Services.Files;

namespace BymseRead.Core.Application.BooksCollection;

[AutoRegistration]
public class BooksCollectionProvider(IBooksQueryRepository repository, IFilesStorageService filesStorageService)
{
    public async Task<BooksCollectionInfo> GetBooks(UserId userId, string? search)
    {
        var books = await repository.GetUserBooks(userId, search);

        var groups = books
            .OrderByDescending(e => e.Progress?.CurrentPageChangeAt)
            .ThenBy(e => e.Book.Title)
            .Select(e => new { book = Build(e), status = BookStatusService.Get(e.Book, e.Progress, e.LastBookmark) })
            .GroupBy(e => e.status)
            .ToArray();

        return new BooksCollectionInfo
        {
            ActiveBooks = ByStatus(BookStatus.Active),
            NewBooks = ByStatus(BookStatus.New),
            TlDrBooks = ByStatus(BookStatus.TlDr),
            ArchivedBooks = ByStatus(BookStatus.Archived),
        };

        BookShortInfo[] ByStatus(BookStatus status)
        {
            return groups
                .FirstOrDefault(e => e.Key == status)
                ?.Select(e => e.book)
                .ToArray() ?? [];
        }
    }

    private BookShortInfo Build(UserBookModel model)
    {
        var lastPage = model.LastBookmark?.Page ?? model.Progress?.CurrentPage ?? 0;

        return new BookShortInfo
        {
            BookId = model.Book.Id.Value,
            Title = model.Book.Title,
            CoverUrl = model.CoverFile != null ? filesStorageService.GetUrl(model.CoverFile) : null,
            FileUrl = filesStorageService.GetUrl(model.BookFile),
            PercentageFinished = lastPage > 0 && model.Book.Pages.HasValue
                ? (int)Math.Round((double)lastPage / model.Book.Pages.Value * 100)
                : 0,
        };
    }
}