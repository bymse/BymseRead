using BymseRead.Core.Application.BooksCollection;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Web.Api.Books;

public class BooksController : WebApiController
{
    public Task<BooksCollectionInfo> GetBooks(
        [FromQuery] string? search,
        [FromServices] BooksCollectionProvider provider
    )
    {
        return provider.GetBooks(CurrentUserId, search);
    }
}