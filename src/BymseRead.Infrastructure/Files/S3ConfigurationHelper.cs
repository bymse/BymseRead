using Microsoft.Extensions.Configuration;

namespace BymseRead.Infrastructure.Files;

public class S3ConfigurationHelper(IConfiguration configuration)
{
    private readonly Uri connectionString = new Uri(configuration.GetConnectionString("BymseReadS3") ??
                                                     throw new InvalidOperationException(
                                                         "Missing S3 connection string"));

    private string? bucketName;

    public string GetBucketName() => bucketName ??= connectionString.LocalPath.Trim('/');

    public S3Config GetS3Config()
    {
        var credentials = connectionString.UserInfo.Split(':');

        return new S3Config(GetUrlBase()
                .ToString(),
            credentials[0],
            credentials[1]);
    }

    public string GetHost() => connectionString.Authority;

    public Uri GetUrlBase() => new($"{connectionString.Scheme}://{GetHost()}");

    public Uri GetPublicUrlBase() => configuration.GetValue<Uri?>("S3FilesStorage:PublicUrlBase") ?? GetUrlBase();
}

public record S3Config(string ServiceUrl, string AccessKey, string SecretKey);