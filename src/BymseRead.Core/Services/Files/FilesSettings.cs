namespace BymseRead.Core.Services.Files;

public class FilesSettings
{
    public const string Path = "Files";

    public required Dictionary<string, long> FileExtensionToMaxSize { get; init; } = new()
    {
        { "pdf", 300 * 1024 * 1024 },
        { "png", 5 * 1024 * 1024 },
        { "jpg", 5 * 1024 * 1024 },
        { "jpeg", 5 * 1024 * 1024 },
    };
}