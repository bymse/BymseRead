using BymseRead.Service.Client.Models;
using BymseRead.Tests.Infrastructure;
using FluentAssertions;
using FileInfo = BymseRead.Service.Client.Models.FileInfo;

namespace BymseRead.Tests.WebApiTests;

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

        const string fileName = "cool-book.pdf";
        const string title = "my cool book";

        var result = await Actions.Books.CreateBook(user, title, fileName: fileName);

        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        book!
            .Should()
            .BeEquivalentTo(new BookInfo
                {
                    BookId = result.BookId, Title = title, BookFile = new FileInfo { Name = fileName, },
                },
                e => e.Excluding(r => r.BookFile!.FileUrl));
    }

    [Test]
    public async Task Should_ReturnValidBookUrl_OnCreatedBook()
    {
        const string content = "my cool content";

        var user = Actions.Users.CreateUser();

        var result = await Actions.Books.CreateBook(user, fileContent: content);

        var client = GetServiceClient(user);
        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        await AssertContent(book!.BookFile!.FileUrl, content);
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