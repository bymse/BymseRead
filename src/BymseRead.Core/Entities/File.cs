namespace BymseRead.Core.Entities;

public record FileId(Guid Value) : IEntityId;

public record FilePath(string Value);

public class File
{
    public const int NameMaxLength = 200;
    public const int PathMaxLength = 2000;

    public FileId Id { get; private init; } = new(Guid.NewGuid());

    public string Name { get; private init; }

    public FilePath Path { get; private init; }

    public long Size { get; private init; }

    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;

    private File()
    {
        Name = null!;
        Path = null!;
    }

    public static File Create(string name, FilePath path, long size)
    {
        return new File
        {
            Name = name, Path = path, Size = size,
        };
    }
}