using System.ComponentModel.DataAnnotations;

namespace BymseBooks.DataLayer.Entity
{
    public class Page
    {
        public int PageId { get; set; }
        
        [StringLength(1024)]
        public string Number { get; set; } = null!;
        public DateTime Date { get; set; }
        public int BookId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        
        public Book Book { get; set; } = null!;
    }
}