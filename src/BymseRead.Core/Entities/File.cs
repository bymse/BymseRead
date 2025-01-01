namespace BymseRead.Core.Entities;

public record FileId(Guid Value) : IEntityId;

public class File
{
    public const int NameMaxLength = 200;
    public const int PathMaxLength = 2000;

    public FileId Id { get; private init; }

    public string Name { get; private init; }

    public string Path { get; private init; }

    public long Size { get; private init; }

    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;

    private File()
    {
        Name = null!;
        Path = null!;
        Id = null!;
    }

    public static File Create(FileId fileId, string name, string path, long size)
    {
        return new File
        {
            Name = name,
            Path = path,
            Size = size,
            Id = fileId,
        };
    }
}