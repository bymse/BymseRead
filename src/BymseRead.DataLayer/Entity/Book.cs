using System.ComponentModel.DataAnnotations;

namespace BymseRead.DataLayer.Entity
{
    public class Book
    {
        public const int MAX_TITLE_LENGTH = 512;
        public const int MAX_AUTHOR_NAME_LENGTH = 512;
        public const int MAX_URL_LENGTH = 512;
        
        public Book()
        {
            BookTags = new HashSet<BookTagLink>();
            Bookmarks = new HashSet<Bookmark>();
        }

        public int BookId { get; set; }

        [Required]
        [MaxLength(MAX_TITLE_LENGTH)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(MAX_AUTHOR_NAME_LENGTH)]
        public string AuthorName { get; set; } = null!;
        
        [MaxLength(MAX_URL_LENGTH)]
        public string? Url { get; set; }
        
        public BookState State { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public int? TotalPages { get; set; }
        
        public virtual ICollection<BookTagLink> BookTags { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
    }
}