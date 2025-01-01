using BymseRead.Core.Common;
using BymseRead.Core.Services;

namespace BymseRead.Core.Application.PrepareFileUpload;

[AutoRegistration]
public class PrepareFileUploadHandler(FilesService filesService)
{
    public async Task<PreparedFileUploadResult> Handle(PrepareFileUploadRequest request)
    {
        return new PreparedFileUploadResult { FileUploadKey = "", UploadUrl = "", };
    }
}