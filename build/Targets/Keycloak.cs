using System;
using Helpers;
using Nuke.Common;
using Nuke.Common.Tools.Docker;
using static Nuke.Common.Tools.Docker.DockerTasks;

partial class Build
{
    const string KeycloakAdminUser = "admin";
    const string KeycloakAdminPassword = "admin";
    const int KeycloakPort = 8080;
    const string KeycloakRealm = "bymse-read";
    const string KeycloakClientId = "bymse-read-service";

    const string KeycloakDefaultUsername = "default";
    const string KeycloakDefaultUserPassword = "default";

    const string OidcCallbackPath = "/web-api/auth/signin-oidc";
    const string OidcSignoutCallbackPath = "/web-api/auth/signout-callback-oidc";

    string KeycloakHost => Environment.GetEnvironmentVariable("KEYCLOAK_HOST") ?? "localhost";
    string KeycloakUrl => $"http://{KeycloakHost}:{KeycloakPort}";
    string MetadataAddress => $"{KeycloakUrl}/realms/{KeycloakRealm}/.well-known/openid-configuration";

    string? ClientSecret;

    Target SetupKeycloak => target => target
        .Executes(async () =>
        {
            var helper = new KeycloakHelper(KeycloakUrl, KeycloakAdminUser, KeycloakAdminPassword);

            ClientSecret = await helper.InitializeKeycloakAsync(
                KeycloakRealm,
                KeycloakClientId,
                LocalUrl,
                [OidcCallbackPath, OidcSignoutCallbackPath],
                KeycloakDefaultUsername,
                KeycloakDefaultUserPassword
            );
        });
}