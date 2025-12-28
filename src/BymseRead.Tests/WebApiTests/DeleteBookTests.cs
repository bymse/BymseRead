using BymseRead.Tests.Infrastructure;
using FluentAssertions;

namespace BymseRead.Tests.WebApiTests;

public class DeleteBookTests : ServiceTestBase
{
    [Test]
    public async Task Should_DoNothing_OnNonExistingBook()
    {
        var user = Actions.Users.CreateUser();

        var nonExistingBookId = Guid.NewGuid();
        await Actions.Books.DeleteBook(user, nonExistingBookId);

        var book = await Actions.Books.GetBook(user, nonExistingBookId);
        book
            .Should()
            .BeNull();
    }

    [Test]
    public async Task Should_DoNothing_OnAnotherUserBook()
    {
        var firstUser = Actions.Users.CreateUser();
        var firstUserBook = await Actions.Books.CreateBook(firstUser);

        var secondUser = Actions.Users.CreateUser();
        await Actions.Books.DeleteBook(secondUser, firstUserBook.BookId!.Value);

        var book = await Actions.Books.GetBook(firstUser, firstUserBook.BookId!.Value);
        book
            .Should()
            .NotBeNull();
    }

    [Test]
    public async Task Should_DeleteBook_OnValidInput()
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var bookResult = await Actions.Books.CreateBook(user);
        await Actions.Books.UpdateBook(user, bookResult.BookId!.Value, "new title", coverFileContent: "image");
        var bookBeforeDelete = await Actions.Books.GetBook(user, bookResult.BookId!.Value);

        await Actions.Books.DeleteBook(user, bookResult.BookId!.Value);

        var book = await client
            .WebApi.Books[bookResult.BookId!.Value]
            .GetAsync();

        book
            .Should()
            .BeNull();

        await AssertNotFound(bookBeforeDelete!.BookFile!.FileUrl);
        await AssertNotFound(bookBeforeDelete.CoverUrl);
    }

    [Test]
    public async Task Should_DeleteBook_OnBookWithProgressAndBookmarks()
    {
        var user = Actions.Users.CreateUser();
        var bookResult = await Actions.Books.CreateBook(user);

        await Actions.Books.AddLastPageBookmark(user, bookResult.BookId!.Value, 1);
        await Actions.Books.AddLastPageBookmark(user, bookResult.BookId!.Value, 2);
        await Actions.Books.UpdateCurrentPage(user, bookResult.BookId!.Value, 1);

        var bookBeforeDelete = await Actions.Books.GetBook(user, bookResult.BookId!.Value);

        await Actions.Books.DeleteBook(user, bookResult.BookId!.Value);

        var bookAfterDelete = await Actions.Books.GetBook(user, bookResult.BookId!.Value);
        bookAfterDelete
            .Should()
            .BeNull();

        await AssertNotFound(bookBeforeDelete!.BookFile!.FileUrl);
        await AssertNotFound(bookBeforeDelete.CoverUrl);
    }
}