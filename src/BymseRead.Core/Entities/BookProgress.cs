namespace BymseRead.Core.Entities;

public class BookProgress
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public BookId BookId { get; init; }
    public UserId UserId { get; init; }
    
    public DateTimeOffset StartedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CurrentPageChangeAt { get; init; }
    
    public int CurrentPage { get; init; }

    private BookProgress()
    {
        BookId = null!;
        UserId = null!;
    }
    
    public static BookProgress Create(BookId book, UserId user)
    {
        return new BookProgress
        {
            BookId = book,
            UserId = user,
        };
    }
}