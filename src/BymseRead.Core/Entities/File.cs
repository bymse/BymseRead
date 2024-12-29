namespace BymseRead.Core.Entities;

public record FileId(Guid Value);

public class File(string name, string path, long size)
{
    public const int NameMaxLength = 200;
    public const int PathMaxLength = 2000; 
    
    public FileId FileId { get; init; } = new(Guid.NewGuid());

    public string Name { get; private init; } = name;

    public string Path { get; private init; } = path;

    public long FileSize { get; private init; } = size;
    
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}