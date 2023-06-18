using BymseRead.DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BymseRead.DataLayer.Database
{
    public partial class BooksDbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookTagLink> BookTagLinks { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
    }
}