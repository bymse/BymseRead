namespace BymseRead.Tools.Tools;

public static class SetupLocalTool
{
    public static async Task<bool> RunAsync()
    {
        try
        {
            Console.WriteLine("Running local setup (Storage + Keycloak + DevConfig)...");
            Console.WriteLine();

            Console.WriteLine("=== Step 1: Storage Setup ===");
            var storageSuccess = await SetupStorageTool.RunAsync();
            if (!storageSuccess)
            {
                Console.Error.WriteLine("Storage setup failed, aborting local setup");
                return false;
            }

            Console.WriteLine();

            Console.WriteLine("=== Step 2: Keycloak Setup ===");
            var keycloakSuccess = await SetupKeycloakTool.RunAsync();
            if (!keycloakSuccess)
            {
                Console.Error.WriteLine("Keycloak setup failed, aborting local setup");
                return false;
            }

            Console.WriteLine();

            Console.WriteLine("=== Step 3: Development Configuration ===");
            var devConfigSuccess = await SetupDevConfigTool.RunAsync();
            if (!devConfigSuccess)
            {
                Console.Error.WriteLine("Development configuration setup failed, aborting local setup");
                return false;
            }

            Console.WriteLine();

            Console.WriteLine("Local setup completed successfully");
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Local setup failed: {ex.Message}");
            return false;
        }
    }
}