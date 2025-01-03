using BymseRead.Core.Entities;

namespace BymseRead.Core.Repositories;

public interface IBooksQueryRepository
{
    Task<IEnumerable<UserBookModel>> GetUserBooks(UserId userId, string? search);
    Task<UserBookModel?> FindUserBook(UserId userId, BookId bookId);
    Task<BookModel?> FindBook(UserId userId, BookId bookId);
    Task<bool> BookExists(UserId userId, BookId bookId);
}

public class UserBookModel
{
    public required Book Book { get; init; }
    public required BookProgress? Progress { get; init; }
    public required Bookmark? LastBookmark { get; init; }
    public required File? CoverFile { get; init; }
    public required File BookFile { get; init; }
}

public class BookModel
{
    public required Book Book { get; init; }
    public required File? CoverFile { get; init; }
    public required File BookFile { get; init; }
}