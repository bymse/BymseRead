using System.Text.Json;
using BymseRead.Tools.Helpers;

namespace BymseRead.Tools.Tools;

public static class SetupDevConfigTool
{
    public static async Task<bool> RunAsync()
    {
        try
        {
            Console.WriteLine("Setting up development configuration...");

            var localUrl = EnvironmentHelper.GetLocalUrl();
            var postgresHost = EnvironmentHelper.GetPostgresHost();
            var keycloakHost = EnvironmentHelper.GetKeycloakHost();
            var minioHost = EnvironmentHelper.GetMinioHost();

            var keycloakUrl = $"http://{keycloakHost}:{Constants.Keycloak.Port}";
            var metadataAddress = $"{keycloakUrl}/realms/{Constants.Keycloak.Realm}/.well-known/openid-configuration";

            Console.WriteLine("Running Keycloak setup to get client secret...");
            var (success, clientSecret) = await SetupKeycloakTool.RunAsync(returnClientSecret: true);

            if (!success || string.IsNullOrEmpty(clientSecret))
            {
                Console.Error.WriteLine("Failed to get client secret from Keycloak");
                return false;
            }

            var config = new
            {
                ConnectionStrings = new
                {
                    BymseReadPostgres = $"Host={postgresHost};Port={Constants.Postgres.Port};" +
                                        $"Database=postgres;Username={Constants.Postgres.User};" +
                                        $"Password={Constants.Postgres.Password}",
                    BymseReadS3 = $"http://{Constants.Storage.RootUser}:{Constants.Storage.RootPassword}" +
                                  $"@{minioHost}:{Constants.Storage.ApiPort}/{Constants.Storage.BucketName}",
                },
                AuthN = new
                {
                    Authority = keycloakUrl,
                    ClientId = Constants.Keycloak.ClientId,
                    ClientSecret = clientSecret,
                    ResponseType = "code",
                    CallbackPath = Constants.Keycloak.OidcCallbackPath,
                    SignedOutCallbackPath = Constants.Keycloak.OidcSignoutCallbackPath,
                    MetadataAddress = metadataAddress
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
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            var serviceDirectory = PathHelper.GetServiceDirectory();
            var configPath = Path.Combine(serviceDirectory, "appsettings.Development.json");

            if (File.Exists(configPath))
            {
                var backupPath = Path.Combine(serviceDirectory, "appsettings.Development.bak.json");
                if (File.Exists(backupPath))
                {
                    Console.WriteLine($"Deleting old backup at {backupPath}");
                    File.Delete(backupPath);
                }

                Console.WriteLine($"Backing up existing config to {backupPath}");
                File.Move(configPath, backupPath);
            }

            Console.WriteLine($"Writing configuration to {configPath}");
            await File.WriteAllTextAsync(configPath, json);

            Console.WriteLine("Development configuration setup completed successfully");
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Development configuration setup failed: {ex.Message}");
            return false;
        }
    }
}