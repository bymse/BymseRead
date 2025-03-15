using Helpers;
using Nuke.Common;
using Nuke.Common.Tools.Docker;
using static Nuke.Common.Tools.Docker.DockerTasks;

partial class Build
{
    const string KeycloakContainerName = "BymseRead.Keycloak";
    const string KeycloakAdminUser = "admin";
    const string KeycloakAdminPassword = "admin";
    const int KeycloakPort = 8080;
    const string KeycloakRealm = "bymse-read";
    const string KeycloakClientId = "bymse-read-service";

    const string KeycloakDefaultUsername = "default";
    const string KeycloakDefaultUserPassword = "default";

    const string OidcCallbackPath = "/oidc/signin-oidc";
    const string OidcSignoutCallbackPath = "/oidc/signout-callback-oidc";

    readonly string KeycloakUrl = $"http://localhost:{KeycloakPort}";

    string? ClientSecret;

    Target UpKeycloak => target => target
        .Executes(() =>
        {
            DockerRun(s => s
                .AddEnv($"KC_BOOTSTRAP_ADMIN_PASSWORD={KeycloakAdminUser}")
                .AddEnv($"KC_BOOTSTRAP_ADMIN_USERNAME={KeycloakAdminPassword}")
                .AddPublish($"{KeycloakPort}:8080")
                .AddVolume($"{KeycloakContainerName}_data:/opt/keycloak/data")
                .SetHealthCmd($"curl --silent --fail {KeycloakUrl}/health/ready || exit 1")
                .SetHealthInterval("30s")
                .SetHealthRetries(5)
                .SetRestart("always")
                .EnableDetach()
                .SetName(KeycloakContainerName)
                .SetImage("quay.io/keycloak/keycloak:26.1.3")
                .SetCommand("start-dev")
            );
        });

    Target SetupKeycloak => target => target
        .After(UpKeycloak)
        .Executes(async () =>
        {
            var helper = new KeycloakHelper(KeycloakUrl, KeycloakAdminUser, KeycloakAdminPassword);

            ClientSecret = await helper.InitializeKeycloakAsync(
                KeycloakRealm,
                KeycloakClientId,
                [OidcCallbackPath, OidcSignoutCallbackPath],
                KeycloakDefaultUsername,
                KeycloakDefaultUserPassword
            );
        });
}