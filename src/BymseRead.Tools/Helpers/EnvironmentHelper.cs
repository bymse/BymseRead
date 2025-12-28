namespace BymseRead.Tools.Helpers;

public static class EnvironmentHelper
{
    public static string GetLocalUrl() =>
        Environment.GetEnvironmentVariable("LOCAL_URL") ?? "http://localhost:5299";

    public static string GetPostgresHost() =>
        Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";

    public static string GetKeycloakHost() =>
        Environment.GetEnvironmentVariable("KEYCLOAK_HOST") ?? "localhost";

    public static string GetMinioHost() =>
        Environment.GetEnvironmentVariable("MINIO_HOST") ?? "localhost";
}