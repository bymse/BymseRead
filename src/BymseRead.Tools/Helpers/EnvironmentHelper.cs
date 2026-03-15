namespace BymseRead.Tools.Helpers;

public static class EnvironmentHelper
{
    public static string GetLocalUrl() => "http://read.bymse.local:5299";

    public static string GetPostgresHost() => "postgres";

    public static string GetKeycloakHost() => "keycloak";

    public static string GetMinioHost() => "minio";
}