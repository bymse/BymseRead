namespace BymseRead.Tools.Helpers;

public static class PathHelper
{
    private static string? _repositoryRoot;

    public static string GetRepositoryRoot()
    {
        if (_repositoryRoot != null)
        {
            return _repositoryRoot;
        }

        var currentDirectory = Directory.GetCurrentDirectory();
        var directory = new DirectoryInfo(currentDirectory);

        while (directory != null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, ".git")))
            {
                _repositoryRoot = directory.FullName;
                return _repositoryRoot;
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