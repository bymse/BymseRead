using BymseBooks.DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BymseBooks.DataLayer.Database
{
    public partial class BooksDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(r =>
            {
                r
                    .HasIndex(e => new { e.Title, e.AuthorName })
                    .IsUnique();

                r
                    .Property(e => e.CreatedDate)
                    .HasConversion<long>();
            });
        }
    }
}