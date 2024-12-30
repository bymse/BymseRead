using Amazon.S3;
using Amazon.S3.Util;
using BymseRead.Core.Common;
using BymseRead.Core.Services;
using File = BymseRead.Core.Entities.File;

namespace BymseRead.Infrastructure.Files;

[AutoRegistration]
public class S3FilesService(IAmazonS3 amazonS3) : IFilesService
{
    public Uri GetUrl(File file)
    {
        return new Uri("https://example.com");
    }
}