using BymseRead.Core.Common;
using BymseRead.Service.Client.Models;
using BymseRead.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Tests.Actions;

[AutoRegistration(Lifetime = ServiceLifetime.Singleton)]
public class BooksActions(ServiceClientProvider provider, FilesActions filesActions)
{
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
}