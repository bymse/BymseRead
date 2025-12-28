namespace BymseRead.Core.Application.PrepareFileUpload;

public class PrepareFileUploadRequest
{
    public required long FileSize { get; init; }
    public required string FileName { get; init; }
}