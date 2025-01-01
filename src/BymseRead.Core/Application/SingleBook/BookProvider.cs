using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using BymseRead.Core.Services;
using BymseRead.Core.Services.Files;

namespace BymseRead.Core.Application.SingleBook;

[AutoRegistration]
public class BookProvider(IBooksQueryRepository repository, IFilesStorageService filesStorageService)
{
    public async Task<BookInfo?> FindBook(UserId userId, BookId bookId)
    {
        var model = await repository.FindUserBook(userId, bookId);
        if (model == null)
        {
            return null;
        }

        return new BookInfo
        {
            BookId = model.Book.Id.Value,
            LastBookmark = MapBookmark(model.LastBookmark),
            Pages = model.Book.Pages,
            Tags = model.Book.Tags,
            Title = model.Book.Title,
            BookFile = MapFile(model.BookFile),
            CoverUrl = model.CoverFile != null ? filesStorageService.GetUrl(model.CoverFile) : null,
            CurrentPage = model.Progress?.CurrentPage,
        };
    }

    private static BookmarkInfo? MapBookmark(Bookmark? bookmark)
    {
        if (bookmark == null)
        {
            return null;
        }

        return new BookmarkInfo { Page = bookmark.Page, CreatedAt = bookmark.CreatedAt, };
    }
    
    private FileInfo MapFile(File file)
    {
        return new FileInfo
        {
            FileUrl = filesStorageService.GetUrl(file), 
            Name = file.Name,
        };
    }
}