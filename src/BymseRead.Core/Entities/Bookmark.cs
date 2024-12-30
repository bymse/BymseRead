namespace BymseRead.Core.Entities;

public class Bookmark
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public BookId BookId { get; init; }
    public UserId UserId { get; init; }

    public BookmarkType Type { get; init; } = BookmarkType.LastPage;

    public int Page { get; set; }

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    private Bookmark()
    {
        BookId = null!;
        UserId = null!;
    }

    public static Bookmark Create(BookId bookId, UserId userId, int page)
    {
        return new Bookmark
        {
            BookId = bookId, UserId = userId, Page = page,
        };
    }
}

public enum BookmarkType
{
    LastPage = 1,
}