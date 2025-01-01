using BymseRead.Core.Application.PrepareFileUpload;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.WebApi.Files;

public class FilesController : WebApiController
{
    [HttpPut("prepare-upload")]
    public PreparedFileUploadResult PrepareUpload(
        [FromBody] PrepareFileUploadRequest request,
        [FromServices] PrepareFileUploadHandler handler
    )
    {
        return handler.Handle(CurrentUserId, request);
    }
}