namespace BymseRead.Infrastructure.Files;

public class S3FilesStorageSettings
{
    public const string Path = "S3FilesStorage";
    
    public required Uri PublicUrlBase { get; init; }
}