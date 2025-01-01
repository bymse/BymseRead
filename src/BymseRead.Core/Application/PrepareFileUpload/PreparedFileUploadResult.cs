namespace BymseRead.Core.Application.PrepareFileUpload;

public class PreparedFileUploadResult
{
    public required string FileUploadKey { get; init; }
    public required string UploadUrl { get; init; }
}