using BymseRead.Core.Entities;

namespace BymseRead.Core.Services.Files;

public interface IFilesStorageService
{
    Uri GetUrl(File file);
    Task<Uri> CreateUploadUrl(UserId userId, string fileUploadKey, string fileName);
    
    Task<UploadedFileModel?> FindUploadedFile(UserId userId, string fileUploadKey);
    
    Task<File> MakePermanent(UserId userId, UploadedFileModel uploadedFile);
}

public class UploadedFileModel
{
    public required string FileName { get; init; }
    public required string Path { get; init; }
    public required long Size { get; init; } 
}