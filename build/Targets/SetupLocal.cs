using System.Linq;
using System.Text.Json;
using Nuke.Common;
using Nuke.Common.IO;

partial class Build
{
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
                    DefaultConnection = $"Host=localhost;Port={LocalPostgresPort};" +
                                        $"Database=postgres;Username={LocalPostgresUser};" +
                                        $"Password={LocalPostgresPassword}"
                }
            };

            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true,
                IndentSize = 2,
            });

            foreach (var path in paths.Where(e => !e.FileExists()))
            {
                path.WriteAllText(json);
            }
        });

    Target SetupLocal => target => target
        .DependsOn(UpDatabase, ApplyMigrations, SetupDevConfig)
        .Executes(() =>
        {
            Serilog.Log.Information("Setup completed");
        });
}