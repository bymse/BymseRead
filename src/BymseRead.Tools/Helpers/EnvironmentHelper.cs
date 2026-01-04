namespace BymseRead.Tools.Helpers;

public static class EnvironmentHelper
{
    public static string GetLocalUrl() =>
        Environment.GetEnvironmentVariable("LOCAL_URL") ?? "http://read.bymse.local:5299";

    public static string GetPostgresHost() =>
        Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";

    public static string GetKeycloakHost() =>
        Environment.GetEnvironmentVariable("KEYCLOAK_HOST") ?? "read.bymse.local";

    public static string GetMinioHost() =>
        Environment.GetEnvironmentVariable("MINIO_HOST") ?? "read.bymse.local";
}