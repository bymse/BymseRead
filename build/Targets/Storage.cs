using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Nuke.Common;
using Nuke.Common.Tools.Docker;
using static Nuke.Common.Tools.Docker.DockerTasks;

partial class Build
{
    private const string StorageContainerName = "BymseRead.Minio";
    private const int StorageApiPort = 19000;
    private const int StorageWebPort = 19001;
    private const string StorageRootUserPassword = "minioadmin";
    private const string StorageRootUser = "minioadmin";
    private const string StorageBucketName = "bymse-read";
    private static readonly string StorageUrl = "http://localhost" + StorageApiPort;

    Target UpStorage => target => target
        .Executes(() =>
        {
            DockerRun(s => s
                .AddEnv($"MINIO_ROOT_USER={StorageRootUser}")
                .AddEnv($"MINIO_ROOT_PASSWORD={StorageRootUserPassword}")
                .AddEnv($"MINIO_SERVER_URL={StorageUrl}")
                .AddVolume($"{StorageContainerName}_data:/data")
                .EnableDetach()
                .AddPublish($"{StorageApiPort}:9000")
                .AddPublish($"{StorageWebPort}:9001")
                .SetRestart("always")
                .SetName(StorageContainerName)
                .SetImage("quay.io/minio/minio:latest")
                .AddArgs("server", "--console-address", ":9001", "/data")
            );
        });

    Target SetupStorage => target => target
        .After(UpStorage)
        .Executes(async () =>
        {
            var client = new AmazonS3Client(new BasicAWSCredentials(StorageRootUser, StorageRootUserPassword),
                new AmazonS3Config
                {
                    ServiceURL = StorageUrl,
                    ForcePathStyle = true,
                });

            var response = await client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = StorageBucketName
            });

            Serilog.Log.Information("Bucket creation response: {0}", response.HttpStatusCode);
        });
}