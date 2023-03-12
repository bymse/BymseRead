using BymseBooks.DataLayer.Entity;

namespace BymseBooks.Core.Models;

public class BookExModel
{
    public BookExModel()
    {
        Book = new BookModel
        {
            Tags = Array.Empty<string>()
        };
        Bookmarks = Array.Empty<BookmarkModel>();
    }
    
    public BookModel Book { get; init; }
    
    public BookmarkModel[] Bookmarks { get; init; }
    
    public string? Url { get; set; }
}