using BymseRead.Core.Entities;

namespace BymseRead.Core.Repositories;

public interface IBooksQueryRepository
{
    Task<IEnumerable<UserBookModel>> GetUserBooks(UserId userId, string? search);
}

public class UserBookModel
{
    public required Book Book { get; init; }
    public required BookProgress? Progress { get; init; }
    public required Bookmark? LastBookmark { get; init; }
    public required File? CoverFile { get; init; }
}