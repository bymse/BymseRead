using System.Text.Json;
using Nuke.Common;
using Nuke.Common.IO;

partial class Build
{
    const string LocalUrl = "http://localhost:5299";
    
    Target SetupDevConfig => target => target
        .DependsOn(SetupKeycloak)
        .Executes(() =>
        {
            var paths = new[]
            {
                Solution.BymseRead_Service.Directory / "appsettings.Development.json",
                Solution.BymseRead_DbMigrator.Directory / "appsettings.Development.json"
            };

            var config = new
            {
                ConnectionStrings = new
                {
                    BymseReadPostgres = $"Host=localhost;Port={LocalPostgresPort};" +
                                        $"Database=postgres;Username={LocalPostgresUser};" +
                                        $"Password={LocalPostgresPassword}",
                    BymseReadS3 = $"http://{StorageRootUser}:{StorageRootUserPassword}" +
                                  $"@localhost:{StorageApiPort}/{StorageBucketName}",
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
    
    Target UpLocal => target => target
        .DependsOn(UpStorage, UpDatabase, UpKeycloak)
        .Executes(() =>
        {
            Serilog.Log.Information("Setup completed");
        });
    
    Target SetupLocal => target => target
        .DependsOn(ApplyMigrations, SetupStorage, SetupKeycloak, SetupDevConfig)
        .Executes(() =>
        {
            Serilog.Log.Information("Setup completed");
        });
}