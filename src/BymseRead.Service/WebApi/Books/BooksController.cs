using BymseRead.Core.Application.BooksCollection;
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
}