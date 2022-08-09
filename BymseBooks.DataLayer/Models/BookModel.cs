using BymseBooks.DataLayer.Entity;

namespace BymseBooks.DataLayer.Models
{
    public class BookModel
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public string? Url { get; set; }
        public IList<int>? TagsIds { get; set; }
        public BookState State { get; set; }
    }
}