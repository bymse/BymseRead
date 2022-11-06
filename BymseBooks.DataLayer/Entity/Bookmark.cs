namespace BymseBooks.DataLayer.Entity;

public class Bookmark
{
    public int BookId { get; set; }
    public int PageNumber { get; set; }
    public string? Title { get; set; }
    
    public BookmarkType BookmarkType { get; set; }

    public virtual Book Book { get; set; }
}