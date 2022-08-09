namespace BymseBooks.DataLayer.Models
{
    public class TagModel
    {
        public int TagId { get; set; }
        public string Title { get; set; } = null!;
        public string ColorCode { get; set; } = null!;
    }
}