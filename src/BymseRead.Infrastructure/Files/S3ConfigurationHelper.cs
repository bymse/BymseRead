using Microsoft.Extensions.Configuration;

namespace BymseRead.Infrastructure.Files;

public class S3ConfigurationHelper(IConfiguration configuration)
{
    private readonly Uri _connectionString = new Uri(configuration.GetConnectionString("BymseReadS3") ??
                                                     throw new InvalidOperationException(
                                                         "Missing S3 connection string"));

    private string? _bucketName;

    public string GetBucketName() => _bucketName ??= _connectionString.LocalPath.Trim('/');

    public S3Config GetS3Config()
    {
        var credentials = _connectionString.UserInfo.Split(':');

        return new S3Config(GetUrlBase()
                .ToString(),
            credentials[0],
            credentials[1]);
    }

    public string GetHost() => _connectionString.Authority;

    public Uri GetUrlBase() => new($"{_connectionString.Scheme}://{GetHost()}");
    
    public Uri GetPublicUrlBase() => configuration.GetValue<Uri?>("S3FilesStorage:PublicUrlBase") ?? GetUrlBase();
}

public record S3Config(string ServiceUrl, string AccessKey, string SecretKey);