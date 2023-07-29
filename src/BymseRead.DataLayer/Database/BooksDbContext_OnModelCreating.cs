using BymseRead.DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BymseRead.DataLayer.Database
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

            modelBuilder.Entity<Bookmark>(r =>
            {
                r
                    .Property(e => e.CreatedDate)
                    .HasConversion<long>();

                r
                    .Property(e => e.ColorCode)
                    .HasDefaultValue(ColorCode.White);
            });
        }
    }
}