using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

#pragma warning disable 8618

namespace BymseBooks.DataLayer.Database
{
    public partial class BooksDbContext : DbContext
    {
        private const string DB_NAME = "Sqlite:Books";

        private readonly IConfiguration config;

        public BooksDbContext(IConfiguration config)
        {
            this.config = config;
        }

        private string ConnectionString =>
            config.GetConnectionString(DB_NAME)
            ?? throw new InvalidOperationException($"No connection string available for {DB_NAME}");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
    }
}