using System.Net;
using Amazon;
using Amazon.Runtime.Internal.Auth;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Util;
using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.Files;
using Microsoft.Extensions.Options;
using File = BymseRead.Core.Entities.File;

namespace BymseRead.Infrastructure.Files;

[AutoRegistration]
public class S3FilesStorageService(IAmazonS3 amazonS3, S3ConfigurationHelper configuration) : IFilesStorageService
{
    private const string OriginalFileNameMetadataKey = "x-amz-meta-originalFileName";

    public Uri GetUrl(File file)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = configuration.GetBucketName(),
            Key = file.Path,
            Expires = DateTime.UtcNow.AddDays(1),
            Verb = HttpVerb.GET,
        };

        var originalRawUrl = amazonS3.GetPreSignedURL(request);
        var originalUrl = new Uri(originalRawUrl);
        
        return new Uri(configuration.GetUrlBase(), originalUrl.PathAndQuery);
    }

    public Uri CreateUploadUrl(UserId userId, string fileUploadKey, string fileName)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = configuration.GetBucketName(),
            Key = GetTempObjectKey(userId, fileUploadKey),
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(60),
            ContentType = "application/octet-stream",
        };
        
        request.Metadata.Add(OriginalFileNameMetadataKey, fileName);
        request.Headers[HeaderKeys.HostHeader] = configuration.GetHost();

        var originalRawUrl = amazonS3.GetPreSignedURL(request);
        var originalUrl = new Uri(originalRawUrl);

        return new Uri(configuration.GetUrlBase(), originalUrl.PathAndQuery);
    }

    public async Task<UploadedFileModel?> FindUploadedFile(UserId userId, string fileUploadKey)
    {
        var key = GetTempObjectKey(userId, fileUploadKey);
        var request = new GetObjectMetadataRequest
        {
            Key = key,
            BucketName = configuration.GetBucketName(),
        };

        try
        {
            var response = await amazonS3.GetObjectMetadataAsync(request);
            return new UploadedFileModel
            {
                FileName = response.Metadata[OriginalFileNameMetadataKey],
                Path = key,
                Size = response.ContentLength,
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
        var copyRequest = new CopyObjectRequest
        {
            SourceKey = uploadedFile.Path,
            SourceBucket = configuration.GetBucketName(),
            DestinationBucket = configuration.GetBucketName(),
            DestinationKey = key,
        };
        await amazonS3.CopyObjectAsync(copyRequest);
        await amazonS3.DeleteObjectAsync(new DeleteObjectRequest
        {
            Key = uploadedFile.Path,
            BucketName = configuration.GetBucketName(),
        });

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