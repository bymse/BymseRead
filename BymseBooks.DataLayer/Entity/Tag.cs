using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BymseBooks.DataLayer.Entity
{
    public class Tag
    {
        public const int MAX_TITLE_LENGTH = 50;
        
        public Tag()
        {
            BookTags = new HashSet<BookTagLink>();
        }
        
        public int TagId { get; set; }
        
        [Required]
        [MaxLength(MAX_TITLE_LENGTH)]
        public string Title { get; set; } = null!;
        
        [Column("ColorCode", TypeName = "char(6)")]
        public string ColorCode { get; set; } = null!;

        public int? CreatorUserId { get; set; }
        
        public User? CreatorUser { get; set; }
        public ICollection<BookTagLink> BookTags { get; private set; }
    }
}