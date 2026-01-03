using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using BymseRead.Core.Services;

namespace BymseRead.Core.Application.UpdateCurrentPage;

[AutoRegistration]
public class UpdateCurrentPageHandler(IBooksQueryRepository booksQueryRepository, IBookProgressRepository bookProgressRepository)
{
    public async Task Handle(UserId userId, BookId bookId, UpdateCurrentPageRequest request)
    {
        var bookExists = await booksQueryRepository.BookExists(userId, bookId);
        if (!bookExists)
        {
            ValidationError.Throw("Book not found");
        }

        if (request.Page < 1)
        {
            ValidationError.Throw("Invalid page number. Page must be greater than 0");
        }

        var progress = BookProgress.Create(bookId, userId, request.Page, request.CreatedAt);
        var rowsAffected = await bookProgressRepository.Upsert(progress);
        if (rowsAffected == 0)
        {
            ValidationError.Throw("A newer progress already exists");
        }
    }
}