using BymseRead.Service.Client.Models;
using BymseRead.Tests.Infrastructure;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace BymseRead.Tests.WebApiTests;

public class BookmarksTests : ServiceTestBase
{
    [Test]
    public async Task Should_Throw_OnNotExistingBook()
    {
        var user = Actions.Users.CreateUser();
        var problem = await Actions
            .Books.Invoking(e => e.AddLastPageBookmark(user, Guid.NewGuid(), 10))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("Book not found");
    }

    [Test]
    public async Task Should_Throw_OnAnotherUsersBook()
    {
        var firstUser = Actions.Users.CreateUser();
        var bookResult = await Actions.Books.CreateBook(firstUser);

        var secondUser = Actions.Users.CreateUser();

        var problem = await Actions
            .Books.Invoking(e => e.AddLastPageBookmark(secondUser, bookResult.BookId!.Value, 10))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("Book not found");
    }

    [TestCase(-1)]
    [TestCase(0)]
    public async Task Should_Throw_OnInvalidPage(int page)
    {
        var user = Actions.Users.CreateUser();
        var bookResult = await Actions.Books.CreateBook(user);

        var problem = await Actions
            .Books.Invoking(e => e.AddLastPageBookmark(user, bookResult.BookId!.Value, page))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("Invalid page number");
    }

    [Test]
    public async Task Should_AddLastPageBookmark_OnValidRequest()
    {
        var user = Actions.Users.CreateUser();
        var bookResult = await Actions.Books.CreateBook(user);

        await Actions.Books.AddLastPageBookmark(user, bookResult.BookId!.Value, 10);

        var book = await Actions.Books.GetBook(user, bookResult.BookId!.Value);

        book!.LastBookmark!
            .Page.Should()
            .Be(10);
    }

    [Test]
    public async Task Should_AddLastPageBookmarks_OnMultipleRequests()
    {
        var user = Actions.Users.CreateUser();
        var bookResult = await Actions.Books.CreateBook(user);

        await Actions.Books.AddLastPageBookmark(user, bookResult.BookId!.Value, 10);
        await Task.Delay(1.Seconds());
        await Actions.Books.AddLastPageBookmark(user, bookResult.BookId!.Value, 15);

        var book = await Actions.Books.GetBook(user, bookResult.BookId!.Value);

        book!.LastBookmark!
            .Page.Should()
            .Be(15);
    }
}