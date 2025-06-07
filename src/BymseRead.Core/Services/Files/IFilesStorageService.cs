using BymseRead.Core.Entities;

namespace BymseRead.Core.Services.Files;

public interface IFilesStorageService
{
    Uri GetUrl(File file);
    PreparedUploadInfo PrepareUpload(UserId userId, string fileName, long fileSize);
    
    Task<UploadedFileModel?> FindUploadedFile(UserId userId, string fileUploadKey);
    
    Task<File> MakePermanent(UserId userId, UploadedFileModel uploadedFile);
    Task Delete(UserId userId, File file);
    
    Task<Stream> Download(File file);
    Task<File> Upload(UserId userId, Stream stream, string fileName);
}

public class UploadedFileModel
{
    public required string FileName { get; init; }
    public required string Path { get; init; }
    public required long Size { get; init; } 
}

public record PreparedUploadInfo(string FileUploadKey, Uri UploadUrl, string EncodedFileName);