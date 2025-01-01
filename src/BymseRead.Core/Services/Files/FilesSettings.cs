namespace BymseRead.Core.Services.Files;

public class FilesSettings
{
    public const string Path = "Files";

    public required FileTypeSettings[] BooksFiles { get; init; } =
    [
        new FileTypeSettings { Extension = "pdf", MaxSize = 300 * 1024 * 1024 },
    ];

    public required FileTypeSettings[] CoverFiles { get; init; } =
    [
        new FileTypeSettings { Extension = "png", MaxSize = 5 * 1024 * 1024 },
        new FileTypeSettings { Extension = "jpg", MaxSize = 5 * 1024 * 1024 },
        new FileTypeSettings { Extension = "jpeg", MaxSize = 5 * 1024 * 1024 },
    ];
}

public class FileTypeSettings
{
    public required string Extension { get; init; }
    public required long MaxSize { get; init; }
}