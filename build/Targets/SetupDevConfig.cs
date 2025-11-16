using System;
using System.Text.Json;
using Nuke.Common;
using Nuke.Common.IO;

partial class Build
{
    string LocalUrl => Environment.GetEnvironmentVariable("LOCAL_URL") ?? "http://localhost:5299";
    string PostgresHost => Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";

    const string LocalPostgresPassword = "postgres";
    const string LocalPostgresUser = "postgres";
    const int LocalPostgresPort = 5432;

    Target SetupDevConfig => target => target
        .DependsOn(SetupKeycloak)
        .Executes(() =>
        {
            var paths = new[]
            {
                Solution.BymseRead_Service.Directory / "appsettings.Development.json",
            };

            var config = new
            {
                ConnectionStrings = new
                {
                    BymseReadPostgres = $"Host={PostgresHost};Port={LocalPostgresPort};" +
                                        $"Database=postgres;Username={LocalPostgresUser};" +
                                        $"Password={LocalPostgresPassword}",
                    BymseReadS3 = $"http://{StorageRootUser}:{StorageRootUserPassword}" +
                                  $"@{StorageHost}:{StorageApiPort}/{StorageBucketName}",
                },
                AuthN = new
                {
                    Authority = KeycloakUrl,
                    ClientId = KeycloakClientId,
                    ClientSecret,
                    ResponseType = "code",
                    CallbackPath = OidcCallbackPath,
                    SignedOutCallbackPath = OidcSignoutCallbackPath,
                    MetadataAddress
                },
                ReturnUrlAllowList = new[]
                {
                    "http://localhost:5173"
                },
                S3FilesStorage = new
                {
                    PublicUrlBase = "http://localhost:5173"
                }
            };

            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true,
                IndentSize = 2,
            });

            foreach (var path in paths)
            {
                if (path.FileExists())
                {
                    var backupPath = path.Parent / $"{path.NameWithoutExtension}.bak.json";
                    if (backupPath.FileExists())
                    {
                        backupPath.DeleteFile();
                    }

                    path.Move(backupPath);
                }

                path.WriteAllText(json);
            }
        });

    Target SetupLocal => target => target
        .DependsOn(SetupStorage, SetupKeycloak, SetupDevConfig)
        .Executes(() =>
        {
            Serilog.Log.Information("Setup completed");
        });
}