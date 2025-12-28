namespace BymseRead.Tools.Helpers;

public static class Constants
{
    public static class Postgres
    {
        public const string User = "postgres";
        public const string Password = "postgres";
        public const int Port = 5432;
    }

    public static class Storage
    {
        public const string RootUser = "minioadmin";
        public const string RootPassword = "minioadmin";
        public const int ApiPort = 9000;
        public const string BucketName = "bymse-read";
    }

    public static class Keycloak
    {
        public const string AdminUser = "admin";
        public const string AdminPassword = "admin";
        public const int Port = 8080;
        public const string Realm = "bymse-read";
        public const string ClientId = "bymse-read-service";
        public const string DefaultUsername = "default";
        public const string DefaultPassword = "default";
        public const string OidcCallbackPath = "/web-api/auth/signin-oidc";
        public const string OidcSignoutCallbackPath = "/web-api/auth/signout-callback-oidc";
    }

    public static class Tools
    {
        public const string DotNetFramework = "net10.0";
    }
}
