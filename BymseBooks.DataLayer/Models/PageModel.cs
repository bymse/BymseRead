namespace BymseBooks.DataLayer.Models
{
    public class PageModel
    {
        public int PageId { get; set; }
        public string Number { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}