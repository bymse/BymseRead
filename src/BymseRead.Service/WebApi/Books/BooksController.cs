using BymseRead.Core.Application.BooksCollection;
using BymseRead.Core.Application.CreateBook;
using BymseRead.Core.Application.DeleteBook;
using BymseRead.Core.Application.SingleBook;
using BymseRead.Core.Application.UpdateBook;
using BymseRead.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.WebApi.Books;

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
    
    [HttpDelete("{bookId:guid}/delete")]
    public Task DeleteBook(
        [FromRoute] Guid bookId,
        [FromServices] DeleteBookHandler handler
    )
    {
        return handler.Handle(CurrentUserId, new BookId(bookId));
    }
}