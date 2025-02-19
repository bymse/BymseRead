using BymseRead.Service.Client.Models;
using BymseRead.Tests.Actions;
using BymseRead.Tests.Infrastructure;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace BymseRead.Tests.WebApiTests;

public class UpdateBookTests : ServiceTestBase
{
    [Test]
    public async Task Should_Throw_OnInvalidBook()
    {
        var user = Actions.Users.CreateUser();
        var problem = await Actions
            .Books.Invoking(e => e.UpdateBook(user, Guid.NewGuid()))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("Book not found");
    }

    [TestCase("")]
    [TestCase("12")]
    [TestCase(
        "1testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest")]
    public async Task Should_Throw_OnInvalidTitle(string title)
    {
        var user = Actions.Users.CreateUser();
        var bookResult = await Actions.Books.CreateBook(user);

        var problem = await Actions
            .Books.Invoking(e => e.UpdateBook(user, bookResult.BookId!.Value, title))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("Title");
    }

    [Test]
    public async Task Should_Throw_OnInvalidBookUploadKey()
    {
        await AssertFileKeyError("random", null, "File not found");
    }

    [Test]
    public async Task Should_Throw_OnInvalidCoverUploadKey()
    {
        await AssertFileKeyError(null, "random", "Cover file not found");
    }

    [Test]
    public async Task Should_Throw_OnAnotherUserBookFile()
    {
        var firstUser = Actions.Users.CreateUser();
        var file = await Actions.Files.UploadFile(firstUser, "file.pdf");

        await AssertFileKeyError(file.FileUploadKey, null, "File not found");
    }

    [Test]
    public async Task Should_Throw_OnAnotherUserCoverFile()
    {
        var firstUser = Actions.Users.CreateUser();
        var file = await Actions.Files.UploadFile(firstUser, "image.png");

        await AssertFileKeyError(null, file.FileUploadKey, "Cover file not found");
    }

    private async Task AssertFileKeyError(string? bookFileUploadKey, string? coverFileUploadKey, string message)
    {
        var user = Actions.Users.CreateUser();
        var bookResult = await Actions.Books.CreateBook(user);

        var client = GetServiceClient(user);

        var problem = await client
            .WebApi.Books[bookResult.BookId!.Value]
            .Update.Invoking(e => e.PostAsync(new UpdateBookRequest
            {
                Title = "test", UploadedBookFileKey = bookFileUploadKey, UploadedCoverFileKey = coverFileUploadKey,
            }))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain(message);
    }

    [Test]
    public async Task Should_UpdateBookTitle_OnPassedTitle()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        const string initialTitle = "initial title";
        const string updatedTitle = "updated title";
        const string bookContent = "book content";

        var result = await Actions.Books.CreateBook(user, initialTitle, fileContent: bookContent);

        await Actions.Books.UpdateBook(user, result.BookId!.Value, updatedTitle);

        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        book!
            .Title.Should()
            .Be(updatedTitle);

        await AssertContent(book.BookFile!.FileUrl, bookContent);
    }

    [Test]
    public async Task Should_ChangeBookFile_OnPassedFileKey()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        const string initialContent = "initial content";
        const string updatedContent = "updated content";
        const string title = "cool book";
        const string fileName = "updated-file.pdf";

        var result = await Actions.Books.CreateBook(user, fileContent: initialContent);
        var bookBeforeUpdate = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        await Actions.Books.UpdateBook(user,
            result.BookId!.Value,
            title,
            bookFileName: fileName,
            bookFileContent: updatedContent);

        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        book!.BookFile!
            .Name.Should()
            .Be(fileName);

        await AssertContent(book.BookFile!.FileUrl, updatedContent);
        await AssertNotFound(bookBeforeUpdate!.BookFile!.FileUrl);
    }

    [Test]
    public async Task Should_UpdateBookCover_OnCoverKeyPassed()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        const string title = "covered book";
        const string coverContent = "cover content";

        var result = await Actions.Books.CreateBook(user, title);

        await Actions.Books.UpdateBook(user, result.BookId!.Value, coverFileContent: coverContent);

        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        await AssertContent(book!.CoverUrl, coverContent);
    }

    [Test]
    public async Task Should_RemoveCover_OnFlagPassed()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        const string title = "covered book";
        const string coverContent = "cover content";

        var result = await Actions.Books.CreateBook(user, title);

        await Actions.Books.UpdateBook(user, result.BookId!.Value, coverFileContent: coverContent);
        var bookBeforeRemove = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        await Actions.Books.UpdateBook(user, result.BookId!.Value, removeCover: true);

        var bookAfterRemove = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        bookAfterRemove!
            .CoverUrl.Should()
            .BeNull();

        await AssertNotFound(bookBeforeRemove!.CoverUrl);
    }

    [Test]
    public async Task Should_ChangeCover_OnRemoveCoverAndUploadedCover()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        const string title = "covered book";
        const string coverContent = "cover content";
        const string newCoverContent = "new cover content";

        var result = await Actions.Books.CreateBook(user, title);

        await Actions.Books.UpdateBook(user, result.BookId!.Value, title, coverFileContent: coverContent);
        var bookBeforeChange = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        await Actions.Books.UpdateBook(user,
            result.BookId!.Value,
            coverFileContent: newCoverContent,
            removeCover: true);

        var book = await client
            .WebApi.Books[result.BookId!.Value]
            .GetAsync();

        await AssertContent(book!.CoverUrl, newCoverContent);
        await AssertNotFound(bookBeforeChange!.CoverUrl);
    }
    
    [Test]
    public async Task Should_ChangeCover_OnBookFileChanged()
    {
        var user = Actions.Users.CreateUser();
        var result = await Actions.Books.CreateBookFromPath(user, FilesActions.FileOtelPdfPath);
        await AssertCover(user, result.BookId!.Value, FilesActions.OtelPdfCoverPath);
        
        await Actions.Books.UpdateBookFromPath(user, result.BookId!.Value, FilesActions.FileModernSePdfPath);
        await AssertCover(user, result.BookId!.Value, FilesActions.FileModernSePdfCoverPath);
    }
}