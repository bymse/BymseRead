using System;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Nuke.Common;
using Nuke.Common.Tools.Docker;
using static Nuke.Common.Tools.Docker.DockerTasks;

partial class Build
{
    private const string StorageContainerName = "BymseRead.Minio";
    private const int StorageApiPort = 9000;
    private const string StorageRootUserPassword = "minioadmin";
    private const string StorageRootUser = "minioadmin";
    private const string StorageBucketName = "bymse-read";
    private string StorageHost => Environment.GetEnvironmentVariable("MINIO_HOST") ?? "localhost";
    private string StorageUrl => $"http://{StorageHost}:{StorageApiPort}";

    Target SetupStorage => target => target
        .Executes(async () =>
        {
            var client = new AmazonS3Client(new BasicAWSCredentials(StorageRootUser, StorageRootUserPassword),
                new AmazonS3Config
                {
                    ServiceURL = StorageUrl,
                    ForcePathStyle = true,
                });

            try
            {
                await client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = StorageBucketName
                });
            }
            catch (BucketAlreadyOwnedByYouException)
            {
                // bucket already exists
            }
        });
}