namespace BymseRead.Tools.Helpers;

public static class PathHelper
{
    private static string? repositoryRoot;

    public static string GetRepositoryRoot()
    {
        if (repositoryRoot != null)
        {
            return repositoryRoot;
        }

        var currentDirectory = Directory.GetCurrentDirectory();
        var directory = new DirectoryInfo(currentDirectory);

        while (directory != null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, ".git")))
            {
                repositoryRoot = directory.FullName;
                return repositoryRoot;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException("Could not find repository root (no .git directory found)");
    }

    public static string GetServiceProjectPath()
    {
        var repoRoot = GetRepositoryRoot();
        return Path.Combine(repoRoot, "src", "BymseRead.Service", "BymseRead.Service.csproj");
    }

    public static string GetServiceDirectory()
    {
        var repoRoot = GetRepositoryRoot();
        return Path.Combine(repoRoot, "src", "BymseRead.Service");
    }

    public static string GetClientProjectPath()
    {
        var repoRoot = GetRepositoryRoot();
        return Path.Combine(repoRoot, "src", "BymseRead.Service.Client", "BymseRead.Service.Client.csproj");
    }

    public static string GetClientDirectory()
    {
        var repoRoot = GetRepositoryRoot();
        return Path.Combine(repoRoot, "src", "BymseRead.Service.Client");
    }

    public static string GetWebClientAppPath()
    {
        var repoRoot = GetRepositoryRoot();
        return Path.Combine(repoRoot, "src", "WebClientApp");
    }
}