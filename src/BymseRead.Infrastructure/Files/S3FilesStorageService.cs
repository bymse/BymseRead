using System.Net;
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
    private const string OriginalFileNameMetadataKey = "x-amz-meta-originalFileName";

    public Uri GetUrl(File file)
    {
        return new Uri("https://example.com");
    }

    public async Task<Uri> CreateUploadUrl(UserId userId, string fileUploadKey, string fileName)
    {
        var request = new GetPreSignedUrlRequest
        {
            Key = GetTempObjectKey(userId, fileUploadKey),
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(60),
            ContentType = "application/octet-stream",
        };

        request.Metadata.Add(OriginalFileNameMetadataKey, fileName);

        var originalRawUrl = await amazonS3.GetPreSignedURLAsync(request);
        var originalUrl = new Uri(originalRawUrl);

        return new Uri(settings.Value.PublicUrlBase, originalUrl.PathAndQuery);
    }

    public async Task<UploadedFileModel?> FindUploadedFile(UserId userId, string fileUploadKey)
    {
        var key = GetTempObjectKey(userId, fileUploadKey);
        var request = new GetObjectMetadataRequest { Key = key, };

        try
        {
            var response = await amazonS3.GetObjectMetadataAsync(request);
            return new UploadedFileModel
            {
                FileName = response.Metadata[OriginalFileNameMetadataKey],
                Path = key,
                Size = long.Parse(response.Metadata["Content-Length"]),
            };
        }
        catch (AmazonS3Exception e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<File> MakePermanent(UserId userId, UploadedFileModel uploadedFile)
    {
        var fileId = new FileId(Guid.NewGuid());
        var key = GetFileObjectKey(userId, fileId);
        var copyRequest = new CopyObjectRequest { SourceKey = uploadedFile.Path, DestinationKey = key, };
        await amazonS3.CopyObjectAsync(copyRequest);
        await amazonS3.DeleteObjectAsync(new DeleteObjectRequest { Key = uploadedFile.Path, });

        return File.Create(fileId, uploadedFile.FileName, key, uploadedFile.Size);
    }

    private static string GetTempObjectKey(UserId userId, string fileUploadKey)
    {
        return $"temp/{userId.Value.ToString()}/{fileUploadKey}";
    }

    private static string GetFileObjectKey(UserId userId, FileId fileId)
    {
        return $"file/{userId.Value.ToString()}/{fileId.Value.ToString()}";
    }
}