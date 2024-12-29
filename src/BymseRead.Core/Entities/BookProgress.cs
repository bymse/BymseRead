namespace BymseRead.Core.Entities;

public class BookProgress(BookId book, UserId user)
{
    public Guid BookProgressId { get; init; } = Guid.NewGuid();
    
    public BookId Book { get; init; } = book;
    public UserId User { get; init; } = user;
    
    public DateTimeOffset StartedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? FinishedAt { get; set; }
    public DateTimeOffset? CurrentPageChangeAt { get; init; }
    
    public int CurrentPage { get; init; }
}