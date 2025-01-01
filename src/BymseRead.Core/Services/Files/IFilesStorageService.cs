using BymseRead.Core.Entities;

namespace BymseRead.Core.Services.Files;

public interface IFilesStorageService
{
    Uri GetUrl(File file);
    Task<Uri> CreateUploadUrl(UserId userId, string fileUploadKey, string fileName);
}