namespace BymseRead.Core.Entities;

public class BookProgress(BookId book, UserId user)
{
    public Guid BookProgressId { get; init; } = Guid.NewGuid();
    
    public BookId Book { get; init; } = book;
    public UserId User { get; init; } = user;
    
    public DateTimeOffset Started { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? Finished { get; set; }
    public DateTimeOffset LastPageChange { get; set; }
    
    public int CurrentPage { get; set; }
}