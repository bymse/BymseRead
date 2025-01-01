using System.Linq;
using System.Text.Json;
using Nuke.Common;
using Nuke.Common.IO;

partial class Build
{
    [Parameter]
    public bool RecreateConfig { get; set; } = false;
    
    Target SetupDevConfig => target => target
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
                                  $"@localhost:{StorageApiPort}/{StorageBucketName}"
                },
                S3FilesStorage = new
                {
                    PublicUrl = $"http://localhost:{StorageApiPort}"
                }
            };

            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true,
                IndentSize = 2,
            });

            foreach (var path in paths.Where(e => RecreateConfig || !e.FileExists()))
            {
                path.WriteAllText(json);
            }
        });

    Target SetupLocal => target => target
        .DependsOn(UpStorage, UpDatabase, ApplyMigrations, SetupStorage, SetupDevConfig)
        .Executes(() =>
        {
            Serilog.Log.Information("Setup completed");
        });
}