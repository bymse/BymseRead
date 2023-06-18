using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

#pragma warning disable 8618

namespace BymseRead.DataLayer.Database
{
    public partial class BooksDbContext : DbContext
    {
        public const string DB_NAME_KEY = "Sqlite:Books";

        private readonly IConfiguration config;

        public BooksDbContext(IConfiguration config)
        {
            this.config = config;
        }

        private string ConnectionString =>
            config.GetConnectionString(DB_NAME_KEY)
            ?? throw new InvalidOperationException($"No connection string available for {DB_NAME_KEY}");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
    }
}