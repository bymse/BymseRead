using BymseRead.Service.Client.Models;
using BymseRead.Tests.Infrastructure;
using FluentAssertions;

namespace BymseRead.Tests;

public class BooksTests : ServiceTestBase
{
    [Test]
    public async Task Should_ReturnEmptyList_OnNewUser()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);
        var collection = await client.WebApi.Books.GetAsync();

        collection!
            .Should()
            .BeEquivalentTo(new BooksCollectionInfo
            {
                ActiveBooks = [],
                NewBooks = [],
                TlDrBooks = [],
                ArchivedBooks = [],
            });
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
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var result = await Actions.Books.CreateBook(user, "my book");

        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        book
            .Should()
            .NotBeNull();
    }

    [Test]
    public async Task Should_ReturnValidBookUrl_OnCreatedBook()
    {
        const string title = "cool book";
        const string content = "my cool content";
        const string fileName = "cool-book.pdf";

        var user = Actions.Users.CreateUser();

        var result = await Actions.Books.CreateBook(user, title, fileName, content);

        var client = GetServiceClient(user);
        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        var fileResponse = await HttpClient.GetAsync(book!.BookFile!.FileUrl);
        fileResponse.EnsureSuccessStatusCode();

        var fileContent = await fileResponse.Content.ReadAsStringAsync();
        fileContent
            .Should()
            .Be(content);
    }

    [Test]
    public async Task Should_ReturnMultipleBooks_OnMultipleCreatedBooks()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var result1 = await Actions.Books.CreateBook(user, "book1");
        var result2 = await Actions.Books.CreateBook(user, "book2");

        var collection = await client.WebApi.Books.GetAsync();

        collection!
            .NewBooks.Should()
            .BeEquivalentTo([
                new BookShortInfo { BookId = result1.BookId, Title = "book1", PercentageFinished = 0 },
                new BookShortInfo { BookId = result2.BookId, Title = "book2", PercentageFinished = 0 },
            ]);
    }
}