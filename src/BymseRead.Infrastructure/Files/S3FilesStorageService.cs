using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.Files;
using File = BymseRead.Core.Entities.File;

namespace BymseRead.Infrastructure.Files;

[AutoRegistration]
public class S3FilesStorageService(IAmazonS3 amazonS3) : IFilesStorageService
{
    public Uri GetUrl(File file)
    {
        return new Uri("https://example.com");
    }

    public async Task<Uri> PrepareUpload(UserId userId, string fileUploadKey)
    {
        var request = new GetPreSignedUrlRequest
        {
            Key = GetUploadObjectKey(userId, fileUploadKey),
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(60),
        };

        var url = await amazonS3.GetPreSignedURLAsync(request);
        return new Uri(url);
    }
    
    private static string GetUploadObjectKey(UserId userId, string fileUploadKey)
    {
        return $"uploads/{userId.Value.ToString()}/{fileUploadKey}";
    }
}