using BymseRead.DataLayer.Database;
using Microsoft.Extensions.Configuration;

namespace BymseRead.App.Models;

public static class ConfigurationSetup
{
    public static void Configure(ConfigurationManager configuration)
    {
        configuration.AddJsonFile("appsettings.json");

#if DEBUG
        configuration.AddJsonFile("appsettings.Debug.json");
#else

        var dbPath = Path.Combine(FileSystem.Current.AppDataDirectory, "books.db");
        configuration.AddInMemoryCollection(new Dictionary<string, string>()
        {
            {$"ConnectionStrings:{BooksDbContext.DB_NAME_KEY}", $"Data Source={dbPath}"}
        });
#endif
    }
}