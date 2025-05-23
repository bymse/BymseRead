using BymseRead.Core.Application.AddLastPageBookmark;
using BymseRead.Core.Application.BooksCollection;
using BymseRead.Core.Application.CreateBook;
using BymseRead.Core.Application.DeleteBook;
using BymseRead.Core.Application.SingleBook;
using BymseRead.Core.Application.UpdateBook;
using BymseRead.Core.Application.UpdateCurrentPage;
using BymseRead.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.WebApi;

public class BooksController : WebApiController
{
    [HttpGet]
    public Task<BooksCollectionInfo> GetBooks(
        [FromQuery] string? search,
        [FromServices] BooksCollectionProvider provider
    )
    {
        return provider.GetBooks(CurrentUserId, search);
    }

    [HttpGet("{bookId:guid}")]
    public Task<BookInfo?> FindBook([FromRoute] Guid bookId, [FromServices] BookProvider provider)
    {
        return provider.FindBook(CurrentUserId, new BookId(bookId));
    }

    [HttpPost]
    public Task<CreatedBookResult> CreateBook(
        [FromBody] CreateBookRequest request,
        [FromServices] CreateBookHandler handler
    )
    {
        return handler.Handle(CurrentUserId, request);
    }

    [HttpPost("{bookId:guid}/update")]
    public Task UpdateBook(
        [FromRoute] Guid bookId,
        [FromBody] UpdateBookRequest request,
        [FromServices] UpdateBookHandler handler
    )
    {
        return handler.Handle(CurrentUserId, new BookId(bookId), request);
    }

    [HttpDelete("{bookId:guid}")]
    public Task DeleteBook([FromRoute] Guid bookId, [FromServices] DeleteBookHandler handler)
    {
        return handler.Handle(CurrentUserId, new BookId(bookId));
    }

    [HttpPost("{bookId:guid}/bookmarks/last-page")]
    public Task AddLastPageBookmark(
        [FromRoute] Guid bookId,
        [FromBody] AddLastPageBookmarkRequest request,
        [FromServices] LastPageBookmarkHandler handler
    )
    {
        return handler.Handle(CurrentUserId, new BookId(bookId), request);
    }
    
    [HttpPut("{bookId:guid}/progress/current-page")]
    public Task UpdateCurrentPage(
        [FromRoute] Guid bookId,
        [FromBody] UpdateCurrentPageRequest request,
        [FromServices] UpdateCurrentPageHandler handler
    )
    {
        return handler.Handle(CurrentUserId, new BookId(bookId), request);
    }
}