using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using BymseRead.Tools.Helpers;

namespace BymseRead.Tools.Tools;

public static class SetupStorageTool
{
    public static async Task<bool> RunAsync()
    {
        try
        {
            Console.WriteLine("Setting up MinIO S3 storage...");

            var storageHost = EnvironmentHelper.GetMinioHost();
            var storageUrl = $"http://{storageHost}:{Constants.Storage.ApiPort}";

            Console.WriteLine($"Connecting to MinIO at {storageUrl}");

            using var client = new AmazonS3Client(
                new BasicAWSCredentials(Constants.Storage.RootUser, Constants.Storage.RootPassword),
                new AmazonS3Config
                {
                    ServiceURL = storageUrl,
                    ForcePathStyle = true,
                });

            try
            {
                Console.WriteLine($"Creating bucket '{Constants.Storage.BucketName}'...");
                await client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = Constants.Storage.BucketName
                });
                Console.WriteLine($"Bucket '{Constants.Storage.BucketName}' created successfully");
            }
            catch (BucketAlreadyOwnedByYouException)
            {
                Console.WriteLine($"Bucket '{Constants.Storage.BucketName}' already exists");
            }

            Console.WriteLine("Storage setup completed successfully");
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Storage setup failed: {ex.Message}");
            return false;
        }
    }
}