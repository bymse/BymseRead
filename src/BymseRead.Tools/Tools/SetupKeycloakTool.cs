using BymseRead.Tools.Helpers;

namespace BymseRead.Tools.Tools;

public static class SetupKeycloakTool
{
    public static async Task<bool> RunAsync()
    {
        var (success, _) = await RunAsync(returnClientSecret: false);
        return success;
    }

    public static async Task<(bool Success, string? ClientSecret)> RunAsync(bool returnClientSecret)
    {
        try
        {
            Console.WriteLine("Setting up Keycloak...");

            var keycloakHost = EnvironmentHelper.GetKeycloakHost();
            var keycloakUrl = $"http://{keycloakHost}:{Constants.Keycloak.Port}";
            var localUrl = EnvironmentHelper.GetLocalUrl();

            Console.WriteLine($"Connecting to Keycloak at {keycloakUrl}");

            using var helper = new KeycloakHelper(keycloakUrl, Constants.Keycloak.AdminUser, Constants.Keycloak.AdminPassword);

            Console.WriteLine($"Initializing realm '{Constants.Keycloak.Realm}'...");
            var clientSecret = await helper.InitializeKeycloakAsync(
                Constants.Keycloak.Realm,
                Constants.Keycloak.ClientId,
                localUrl,
                [Constants.Keycloak.OidcCallbackPath, Constants.Keycloak.OidcSignoutCallbackPath],
                Constants.Keycloak.DefaultUsername,
                Constants.Keycloak.DefaultPassword
            );

            Console.WriteLine($"Realm '{Constants.Keycloak.Realm}' initialized successfully");
            Console.WriteLine($"Client '{Constants.Keycloak.ClientId}' created");
            Console.WriteLine($"Default user '{Constants.Keycloak.DefaultUsername}' created");

            if (returnClientSecret)
            {
                Console.WriteLine($"Client secret: {clientSecret}");
            }

            Console.WriteLine("Keycloak setup completed successfully");
            return (true, clientSecret);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Keycloak setup failed: {ex.Message}");
            return (false, null);
        }
    }
}