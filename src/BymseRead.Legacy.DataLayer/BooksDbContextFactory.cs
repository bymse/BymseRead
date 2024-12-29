using BymseRead.Legacy.DataLayer.Database;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BymseRead.Legacy.DataLayer;

public class BooksDbContextFactory : IDesignTimeDbContextFactory<BooksDbContext>
{
    public BooksDbContext CreateDbContext(string[] args)
    {
        var debugConfigPath = Path.Combine(
            Directory.GetCurrentDirectory(), "..", "BymseRead.App", "appsettings.Debug.json"
        );
        var config = new ConfigurationBuilder()
            .AddJsonFile(debugConfigPath)
            .Build();

        return new BooksDbContext(config);
    }
}