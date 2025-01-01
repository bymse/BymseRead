using System.Net.Http.Headers;
using System.Text;
using BymseRead.Service.Client.Models;
using BymseRead.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Kiota.Abstractions;
using FileInfo = BymseRead.Service.Client.Models.FileInfo;

namespace BymseRead.Tests;

public class BooksTests : ServiceTestBase
{
    [Test]
    public async Task Should_ReturnEmptyList_OnNewUser()
    {
        var client = GetServiceClient();
        var collection = await client.WebApi.Books.GetAsync();

        collection
            .Should()
            .NotBeNull();

        collection!
            .ActiveBooks.Should()
            .BeEmpty();

        collection
            .NewBooks.Should()
            .BeEmpty();

        collection
            .TlDrBooks.Should()
            .BeEmpty();

        collection
            .ArchivedBooks.Should()
            .BeEmpty();
    }

    [Test]
    public async Task Should_ReturnEmptyResponse_OnNotExistingBook()
    {
        var client = GetServiceClient();
        var book = await client
            .WebApi.Books[Guid.NewGuid()]
            .GetAsync();

        book
            .Should()
            .BeNull();
    }

    [Test]
    public async Task Should_CreateBook_OnValidInput()
    {
        const string fileName = "book.pdf";
        const string bookTitle = "my book";
        var client = GetServiceClient();
        using var content = new StringContent(new string('x', 10));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Headers.Add("x-amz-meta-originalFileName", fileName);

        var result = await client.WebApi.Files.PrepareUpload.PutAsync(new PrepareFileUploadRequest
        {
            FileName = fileName, FileSize = 10,
        });

        var response = await HttpClient.PutAsync(new Uri(result!.UploadUrl!), content);
        response.EnsureSuccessStatusCode();

        var createdBook = await client.WebApi.Books.PostAsync(new CreateBookRequest
        {
            Title = bookTitle, FileUploadKey = result.FileUploadKey,
        });

        createdBook
            .Should()
            .NotBeNull();

        var book = await client
            .WebApi.Books[createdBook!.BookId!.Value]
            .GetAsync();

        book
            .Should()
            .BeEquivalentTo(new BookInfo
            {
                Title = bookTitle,
                BookId = createdBook.BookId,
                Pages = 0,
                Tags = [],
                CoverUrl = null,
                CurrentPage = null,
                LastBookmark = null,
                BookFile = new FileInfo { Name = fileName, FileUrl = "", },
            });
    }
}