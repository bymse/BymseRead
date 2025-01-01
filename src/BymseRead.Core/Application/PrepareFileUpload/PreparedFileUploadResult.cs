namespace BymseRead.Core.Application.PrepareFileUpload;

public class PreparedFileUploadResult
{
    public required string FileUploadKey { get; init; }
    public required Uri UploadUrl { get; init; }
}