namespace BymseRead.Core.Entities;

public class BookProgress
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public BookId BookId { get; init; }
    public UserId UserId { get; init; }

    public DateTimeOffset StartedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CurrentPageChangeAt { get; init; } = DateTimeOffset.UtcNow;

    public int CurrentPage { get; set; }

    private BookProgress()
    {
        BookId = null!;
        UserId = null!;
    }

    public static BookProgress Create(BookId book, UserId user, int page)
    {
        return new BookProgress
        {
            BookId = book,
            UserId = user,
            CurrentPage = page,
        };
    }
}