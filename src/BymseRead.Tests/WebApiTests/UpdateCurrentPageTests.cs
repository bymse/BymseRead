using BymseRead.Service.Client.Models;
using BymseRead.Tests.Infrastructure;
using FluentAssertions;

namespace BymseRead.Tests.WebApiTests;

public class UpdateCurrentPageTests : ServiceTestBase
{
    [Test]
    public async Task Should_Throw_OnNotExistingBook()
    {
        var user = Actions.Users.CreateUser();
        var problem = await Actions
            .Books.Invoking(e => e.UpdateCurrentPage(user, Guid.NewGuid(), 10))
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
            .Books.Invoking(e => e.UpdateCurrentPage(secondUser, bookResult.BookId!.Value, 10))
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
            .Books.Invoking(e => e.UpdateCurrentPage(user, bookResult.BookId!.Value, page))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("Invalid page number");
    }

    [Test]
    public async Task Should_UpdateCurrentPage_OnValidRequest()
    {
        var user = Actions.Users.CreateUser();
        var bookResult = await Actions.Books.CreateBook(user);

        await Actions.Books.UpdateCurrentPage(user, bookResult.BookId!.Value, 10);

        var book = await Actions.Books.GetBook(user, bookResult.BookId!.Value);
        book!.CurrentPage.Should().Be(10);
    }

    [Test]
    public async Task Should_UpdateCurrentPage_OnMultipleRequests()
    {
        var user = Actions.Users.CreateUser();
        var bookResult = await Actions.Books.CreateBook(user);

        await Actions.Books.UpdateCurrentPage(user, bookResult.BookId!.Value, 10);
        await Actions.Books.UpdateCurrentPage(user, bookResult.BookId!.Value, 20);

        var book = await Actions.Books.GetBook(user, bookResult.BookId!.Value);
        book!.CurrentPage.Should().Be(20);
    }
}