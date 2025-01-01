using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.Files;

namespace BymseRead.Core.Application.PrepareFileUpload;

[AutoRegistration]
public class PrepareFileUploadHandler(IFilesStorageService filesStorage, FilesValidator filesValidator)
{
    public async Task<PreparedFileUploadResult> Handle(UserId userId, PrepareFileUploadRequest request)
    {
        filesValidator.Validate(request.FileName, request.FileSize);

        var uploadKey = Guid
            .NewGuid()
            .ToString();
        
        var uploadUrl = await filesStorage.CreateUploadUrl(userId, uploadKey, request.FileName);
        
        return new PreparedFileUploadResult
        {
            FileUploadKey = uploadKey,
            UploadUrl = uploadUrl,
        };
    }
}