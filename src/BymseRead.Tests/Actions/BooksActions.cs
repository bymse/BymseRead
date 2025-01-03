using BymseRead.Core.Common;
using BymseRead.Service.Client.Models;
using BymseRead.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Tests.Actions;

[AutoRegistration(Lifetime = ServiceLifetime.Singleton)]
public class BooksActions(ServiceClientProvider provider, FilesActions filesActions)
{
    public async Task AddLastPageBookmark(Guid userId, Guid bookId, int page)
    {
        var client = provider.Get(userId);

        await client
            .WebApi.Books[bookId]
            .Bookmarks.LastPage
            .PostAsync(new AddLastPageBookmarkRequest { Page = page });
    }
    
    public async Task<BookInfo?> GetBook(Guid userId, Guid bookId)
    {
        var client = provider.Get(userId);
        return await client
            .WebApi.Books[bookId]
            .GetAsync();
    }

    public async Task<CreatedBookResult> CreateBook(
        Guid userId,
        string title = "my book",
        string fileName = "book.pdf",
        string fileContent = "hello world"
    )
    {
        var result = await filesActions.UploadFile(userId, fileName, fileContent);

        var createdBook = await provider
            .Get(userId)
            .WebApi.Books.PostAsync(new CreateBookRequest { Title = title, FileUploadKey = result.FileUploadKey, });

        return createdBook!;
    }

    public async Task DeleteBook(Guid userId, Guid bookId)
    {
        var client = provider.Get(userId);

        await client
            .WebApi.Books[bookId]
            .DeleteAsync();
    }

    public async Task UpdateBook(
        Guid userId,
        Guid bookId,
        string title = "my book",
        string? bookFileName = null,
        string? bookFileContent = null,
        string? coverFileContent = null,
        bool removeCover = false
    )
    {
        PreparedFileUploadResult? coverFile = null;
        PreparedFileUploadResult? bookFile = null;
        if (coverFileContent != null)
        {
            coverFile = await filesActions.UploadFile(userId, "cover.jpg", coverFileContent);
        }

        if (bookFileContent != null)
        {
            bookFile = await filesActions.UploadFile(userId, bookFileName!, bookFileContent);
        }

        var client = provider.Get(userId);

        await client
            .WebApi.Books[bookId]
            .Update.PostAsync(new UpdateBookRequest
            {
                Title = title,
                UploadedCoverFileKey = coverFile?.FileUploadKey,
                UploadedBookFileKey = bookFile?.FileUploadKey,
                RemoveCover = removeCover,
            });
    }
}