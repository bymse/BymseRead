using BymseRead.Core.Entities;

namespace BymseRead.Core.Services.Files;

public interface IFilesStorageService
{
    Uri GetUrl(File file);
    Uri CreateUploadUrl(UserId userId, string fileUploadKey, string fileName, long fileSize);
    
    Task<UploadedFileModel?> FindUploadedFile(UserId userId, string fileUploadKey);
    
    Task<File> MakePermanent(UserId userId, UploadedFileModel uploadedFile);
    Task Delete(UserId userId, File file);
}

public class UploadedFileModel
{
    public required string FileName { get; init; }
    public required string Path { get; init; }
    public required long Size { get; init; } 
}