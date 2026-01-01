using BymseRead.Service.Client.Models;
using BymseRead.Tests.Actions;
using BymseRead.Tests.Infrastructure;
using FluentAssertions;
using FluentAssertions.Extensions;
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

    [TestCase("")]
    [TestCase("12")]
    [TestCase(
        "1testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest")]
    public async Task Should_ReturnError_OnInvalidTitle(string title)
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var problem = await client
            .WebApi.Books.Invoking(e => e.PostAsync(new CreateBookRequest { Title = title, FileUploadKey = "", }))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("Title");
    }

    [Test]
    public async Task Should_ReturnError_OnInvalidFileUploadKey()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var problem = await client
            .WebApi.Books
            .Invoking(e => e.PostAsync(new CreateBookRequest { Title = "test", FileUploadKey = "random", }))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("File not found");
    }

    [Test]
    public async Task Should_ReturnError_OnAnotherUserFile()
    {
        var firstUser = Actions.Users.CreateUser();
        var file = await Actions.Files.UploadFile(firstUser, "file.pdf");

        var secondUser = Actions.Users.CreateUser();
        var secondClient = GetServiceClient(secondUser);

        var problem = await secondClient
            .WebApi.Books.Invoking(e =>
                e.PostAsync(new CreateBookRequest { Title = "test", FileUploadKey = file.FileUploadKey, }))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("File not found");
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
                BookId = result.BookId,
                Title = title,
                BookFile = new FileInfo { Name = fileName, },
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
                new BookCollectionItem { BookId = result1.BookId, Title = "book1", PercentageFinished = 0, CurrentPage = null, LastBookmark = null },
                new BookCollectionItem { BookId = result2.BookId, Title = "book2", PercentageFinished = 0, CurrentPage = null, LastBookmark = null },
            ], e => e.Excluding(r => r.FileUrl).Excluding(r => r.CoverUrl));
    }

    [Test]
    public async Task Should_UpdatePagesInBackground_OnBookCreated()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var result = await Actions.Books.CreateBookFromPath(user, FilesActions.FileOtelPdfPath);

        await Task.Delay(10.Seconds());

        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        book!
            .Pages.Should()
            .Be(4);
    }

    [Test]
    public async Task Should_UpdateCoverInBackground_OnUpdateCurrentPage()
    {
        var user = Actions.Users.CreateUser();
        var result = await Actions.Books.CreateBookFromPath(user, FilesActions.FileOtelPdfPath);
        await AssertCover(user, result.BookId!.Value, FilesActions.OtelPdfCoverPath);
    }

    [Test]
    public async Task Should_ReturnFileUrl_OnBookInCollection()
    {
        const string content = "book file content";

        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var result = await Actions.Books.CreateBook(user, fileContent: content);

        var collection = await client.WebApi.Books.GetAsync();

        var bookCollectionItem = collection!.NewBooks.Should().ContainSingle(e => e.BookId == result.BookId).Subject;
        bookCollectionItem.FileUrl.Should().NotBeNullOrEmpty();
        await AssertContent(bookCollectionItem.FileUrl, content);
    }

    [Test]
    public async Task Should_ReturnNewStatus_OnNewBook()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var result = await Actions.Books.CreateBook(user);

        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        book!.Status.Should().Be(BookStatus.New);
    }

    [Test]
    public async Task Should_ReturnActiveStatus_OnBookWithProgress()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var result = await Actions.Books.CreateBook(user);
        await Actions.Books.UpdateCurrentPage(user, result.BookId!.Value, 5);

        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        book!.Status.Should().Be(BookStatus.Active);
    }

    [Test]
    public async Task Should_ReturnArchivedStatus_OnBookNearCompletion()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var result = await Actions.Books.CreateBookFromPath(user, FilesActions.FileOtelPdfPath);
        await Task.Delay(10.Seconds());

        await Actions.Books.UpdateCurrentPage(user, result.BookId!.Value, 3);

        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        book!.Status.Should().Be(BookStatus.Archived);
    }

    [Test]
    public async Task Should_ReturnCurrentPageAndLastBookmark_InCollection()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var result = await Actions.Books.CreateBook(user);
        await Actions.Books.UpdateCurrentPage(user, result.BookId!.Value, 5);
        await Actions.Books.AddLastPageBookmark(user, result.BookId!.Value, 3);

        var collection = await client.WebApi.Books.GetAsync();

        var bookItem = collection!.ActiveBooks.Should().ContainSingle(e => e.BookId == result.BookId).Subject;
        bookItem.CurrentPage.Should().Be(5);
        bookItem.LastBookmark.Should().NotBeNull();
        bookItem.LastBookmark!.Page.Should().Be(3);
    }
}