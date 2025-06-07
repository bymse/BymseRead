using Nuke.Common;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.Docker.DockerTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    const string ContainerName = "BymseRead.Postgres";
    const string LocalPostgresPassword = "postgres";
    const string LocalPostgresUser = "postgres";
    const int LocalPostgresPort = 15432;

    Target UpDatabase => target => target
        .Executes(() =>
        {
            DockerRun(s => s
                .AddEnv($"POSTGRES_PASSWORD={LocalPostgresPassword}")
                .AddEnv($"POSTGRES_USER={LocalPostgresUser}")
                .AddVolume($"{ContainerName}_pgdata:/var/lib/postgresql/data")
                .AddPublish($"{LocalPostgresPort}:5432")
                .SetRestart("always")
                .EnableDetach()
                .SetName(ContainerName)
                .SetImage("postgres:17")
            );
        });

    Target ApplyMigrations => target => target
        .DependsOn(SetupDevConfig)
        .After(UpDatabase)
        .Executes(() =>
        {
            DotNetRun(e => e
                .SetProjectFile(Solution.BymseRead_DbMigrator.Path)
                .SetConfiguration(Configuration)
                .SetLaunchProfile("DbMigrator"));
        });
}