using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Util;
using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.Files;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using File = BymseRead.Core.Entities.File;

namespace BymseRead.Infrastructure.Files;

[AutoRegistration(Lifetime = ServiceLifetime.Singleton)]
public class S3FilesStorageService(
    IAmazonS3 amazonS3,
    S3ConfigurationHelper configuration,
    IMemoryCache memoryCache,
    ILogger<S3FilesStorageService> logger
) : IFilesStorageService
{
    private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();

    private const string OriginalFileNameMetadataKey = "x-amz-meta-originalFileName";

    public Uri GetUrl(File file)
    {
        var cacheKey = $"S3:GetUrl:{file.Id.Value}";
        if (memoryCache.TryGetValue(cacheKey, out Uri? url) && url != null)
        {
            return url;
        }

        var extension = Path.GetExtension(file.Name);
        var request = new GetPreSignedUrlRequest
        {
            BucketName = configuration.GetBucketName(),
            Key = file.Path,
            Expires = DateTime.UtcNow.AddHours(24),
            Verb = HttpVerb.GET,
            ResponseHeaderOverrides =
            {
                // 12 hours
                CacheControl = "private, max-age=43200, immutable",
                ContentType =
                    !string.IsNullOrEmpty(extension) && ContentTypeProvider.TryGetContentType(extension, out var type)
                        ? type
                        : null,
            },
        };

        var originalRawUrl = amazonS3.GetPreSignedURL(request);
        var originalUrl = new Uri(originalRawUrl);

        var resultUrl = new Uri(configuration.GetPublicUrlBase(), originalUrl.PathAndQuery);
        memoryCache.Set(cacheKey, resultUrl, TimeSpan.FromHours(12));

        return resultUrl;
    }

    public PreparedUploadInfo PrepareUpload(UserId userId, string fileName, long fileSize)
    {
        var extension = Path.GetExtension(fileName);
        var fileUploadKey = $"{Guid.NewGuid()}{extension}";

        var request = new GetPreSignedUrlRequest
        {
            BucketName = configuration.GetBucketName(),
            Key = GetTempObjectKey(userId, fileUploadKey),
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(60),
            ContentType = "application/octet-stream",
        };

        var encodedFileName = EncodeFileName(fileName);
        request.Metadata.Add(OriginalFileNameMetadataKey, encodedFileName);
        request.Headers[HeaderKeys.HostHeader] = configuration.GetHost();
        request.Headers[HeaderKeys.ContentLengthHeader] = fileSize.ToString();

        var originalRawUrl = amazonS3.GetPreSignedURL(request);
        var originalUrl = new Uri(originalRawUrl);

        var uploadUrl = new Uri(configuration.GetPublicUrlBase(), originalUrl.PathAndQuery);
        return new PreparedUploadInfo(fileUploadKey, uploadUrl, encodedFileName);
    }

    public async Task<UploadedFileModel?> FindUploadedFile(UserId userId, string fileUploadKey)
    {
        var key = GetTempObjectKey(userId, fileUploadKey);
        var request = new GetObjectMetadataRequest { Key = key, BucketName = configuration.GetBucketName(), };

        try
        {
            var response = await amazonS3.GetObjectMetadataAsync(request);
            return new UploadedFileModel
            {
                FileName = DecodeFileName(response.Metadata?[OriginalFileNameMetadataKey]),
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
        var extension = Path.GetExtension(uploadedFile.FileName);
        var key = GetFileObjectKey(userId, fileId, extension);
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

    public Task Delete(UserId userId, File file)
    {
        var request = new DeleteObjectRequest { Key = file.Path, BucketName = configuration.GetBucketName(), };

        return amazonS3.DeleteObjectAsync(request);
    }

    public async Task<Stream> Download(File file)
    {
        var request = new GetObjectRequest { Key = file.Path, BucketName = configuration.GetBucketName(), };

        var response = await amazonS3.GetObjectAsync(request);
        return response.ResponseStream;
    }

    public async Task<File> Upload(UserId userId, Stream stream, string fileName)
    {
        var fileId = new FileId(Guid.NewGuid());
        var extension = Path.GetExtension(fileName);
        var key = GetFileObjectKey(userId, fileId, extension);
        var request = new PutObjectRequest
        {
            BucketName = configuration.GetBucketName(),
            Key = key,
            InputStream = stream,
            ContentType = "application/octet-stream",
            Metadata = { [OriginalFileNameMetadataKey] = EncodeFileName(fileName) },
            AutoCloseStream = false,
        };

        await amazonS3.PutObjectAsync(request);

        return File.Create(fileId, fileName, key, stream.Length);
    }

    public async Task<bool> IsBucketAvailable(CancellationToken ct)
    {
        var bucketName = configuration.GetBucketName();
        try
        {
            var response = await amazonS3.GetBucketLocationAsync(bucketName, ct);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                logger.LogWarning("Bucket {BucketName} is not available. Status code: {StatusCode}",
                    bucketName,
                    response.HttpStatusCode);

                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to check bucket availability for bucket {BucketName}", bucketName);
            return false;
        }
    }

    private static string GetTempObjectKey(UserId userId, string fileUploadKey)
    {
        return $"temp/{userId.Value}/{fileUploadKey}";
    }

    private static string GetFileObjectKey(UserId userId, FileId fileId, string extension)
    {
        return $"file/{userId.Value}/{fileId.Value}{extension}";
    }

    private static string EncodeFileName(string fileName)
    {
        return WebUtility.UrlEncode(fileName);
    }

    private static string DecodeFileName(string? fileName)
    {
        return string.IsNullOrEmpty(fileName) ? "" : WebUtility.UrlDecode(fileName);
    }
}