namespace BymseRead.Legacy.DataLayer.Entity
{
    public class BookTagLink
    {
        public int BookTagLinkId { get; set; }
        public int BookId { get; set; }
        public int TagId { get; set; }
        
        public Tag Tag { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}