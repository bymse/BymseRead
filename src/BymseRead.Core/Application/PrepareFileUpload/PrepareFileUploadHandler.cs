using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.Files;

namespace BymseRead.Core.Application.PrepareFileUpload;

[AutoRegistration]
public class PrepareFileUploadHandler(IFilesStorageService filesStorage, FilesValidator filesValidator)
{
    public PreparedFileUploadResult Handle(UserId userId, PrepareFileUploadRequest request)
    {
        filesValidator.Validate(request.FileName, request.FileSize);

        var model = filesStorage.PrepareUpload(userId, request.FileName, request.FileSize);

        return new PreparedFileUploadResult
        {
            FileUploadKey = model.FileUploadKey,
            UploadUrl = model.UploadUrl,
            EncodedFileName = model.EncodedFileName
        };
    }
}