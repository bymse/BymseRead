using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using BymseRead.Core.Services;

namespace BymseRead.Core.Application.AddLastPageBookmark;

[AutoRegistration]
public class LastPageBookmarkHandler(
    IBooksQueryRepository booksQueryRepository,
    IBookmarksRepository bookmarksRepository
)
{
    public async Task Handle(UserId userId, BookId bookId, AddLastPageBookmarkRequest request)
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

        var bookmark = Bookmark.Create(bookId, userId, request.Page);

        await bookmarksRepository.Add(bookmark);
    }
}