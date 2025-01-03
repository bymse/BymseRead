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
        var book = await booksQueryRepository.FindBook(userId, bookId);
        if (book == null)
        {
            ValidationError.Throw("Book not found");
        }

        var bookmark = Bookmark.Create(bookId, userId, request.Page);

        await bookmarksRepository.Add(bookmark);
    }
}