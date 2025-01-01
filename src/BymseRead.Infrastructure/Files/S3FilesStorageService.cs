using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.Files;
using Microsoft.Extensions.Options;
using File = BymseRead.Core.Entities.File;

namespace BymseRead.Infrastructure.Files;

[AutoRegistration]
public class S3FilesStorageService(IAmazonS3 amazonS3, IOptions<S3FilesStorageSettings> settings) : IFilesStorageService
{
    public Uri GetUrl(File file)
    {
        return new Uri("https://example.com");
    }

    public async Task<Uri> CreateUploadUrl(UserId userId, string fileUploadKey, string fileName)
    {
        var request = new GetPreSignedUrlRequest
        {
            Key = GetUploadObjectKey(userId, fileUploadKey),
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(60),
        };
        
        request.Metadata.Add("originalFileName", fileName);

        var originalRawUrl = await amazonS3.GetPreSignedURLAsync(request);
        var originalUrl = new Uri(originalRawUrl);
        
        return new Uri(settings.Value.PublicUrlBase, originalUrl.PathAndQuery);
    }
    
    private static string GetUploadObjectKey(UserId userId, string fileUploadKey)
    {
        return $"uploads/{userId.Value.ToString()}/{fileUploadKey}";
    }
}