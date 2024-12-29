namespace BymseRead.Core.Entities;

public record BookId(Guid Value);

public class Book(string title, FileId bookFile, UserId ownerUser)
{
    public const int MaxTitleLength = 200;
    
    public BookId BookId { get; init; } = new(Guid.NewGuid());

    public string Title { get; private set; } = title;

    public string[] Tags { get; set; } = [];
    
    public int Pages { get; set; }

    public FileId BookFile { get; set; } = bookFile;

    public FileId? BookCoverFile { get; set; }

    public UserId OwnerUser { get; set; } = ownerUser;
    
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}