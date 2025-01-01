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
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);
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
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);
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
        const string fileContent = "hello world";
        
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);
        
        using var content = new StringContent(fileContent);
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
                    BookFile = new FileInfo { Name = fileName, },
                },
                e => e.Excluding(r => r.BookFile!.FileUrl));
        
        var downloadedFileResponse = await HttpClient.GetAsync(new Uri(book!.BookFile!.FileUrl!));
        downloadedFileResponse.EnsureSuccessStatusCode();
        
        var downloadedFileContent = await downloadedFileResponse.Content.ReadAsStringAsync();
        downloadedFileContent
            .Should()
            .Be(fileContent);
    }
}