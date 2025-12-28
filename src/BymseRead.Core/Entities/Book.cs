namespace BymseRead.Core.Entities;

public record BookId(Guid Value) : IEntityId;

public class Book
{
    public const int MaxTitleLength = 200;

    public static Book Create(string title, FileId bookFileId, UserId ownerUserId)
    {
        return new Book
        {
            Title = title,
            BookFileId = bookFileId,
            OwnerUserId = ownerUserId
        };
    }

    public BookId Id { get; init; } = new(Guid.NewGuid());

    public string Title { get; set; }

    public int? Pages { get; set; }

    public FileId BookFileId { get; set; }

    public FileId? BookCoverFileId { get; set; }

    public UserId OwnerUserId { get; set; }

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    private Book()
    {
        Title = null!;
        BookFileId = null!;
        OwnerUserId = null!;
    }
}