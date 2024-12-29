namespace BymseRead.Core.Entities;

public class Bookmark(BookId book, UserId user, int page)
{
    public Guid BookmarkId { get; init; } = Guid.NewGuid();
    
    public BookId Book { get; init; } = book;
    public UserId User { get; init; } = user;
    
    public BookmarkType Type { get; } = BookmarkType.LastPage;
    
    public int Page { get; set; } = page;
    
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}

public enum BookmarkType
{
    LastPage = 1,
}