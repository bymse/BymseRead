namespace BymseRead.Core.Entities;

public record BookId(Guid Value);

public class Book(string title, FileId bookFile, UserId ownerUser)
{
    public BookId BookId { get; init; } = new(Guid.NewGuid());

    public string Title { get; private set; } = title;

    public string[] Tags { get; set; } = [];

    public FileId BookFile { get; set; } = bookFile;

    public FileId? BookCoverFile { get; set; }

    public UserId OwnerUser { get; set; } = ownerUser;
}