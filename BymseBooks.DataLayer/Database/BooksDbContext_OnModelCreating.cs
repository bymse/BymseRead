using BymseBooks.DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BymseBooks.DataLayer.Database
{
    public partial class BooksDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<Book>()
                .HasIndex(e => new {e.Title, e.AuthorName, e.UserId})
                .IsUnique();
        }
    }
}