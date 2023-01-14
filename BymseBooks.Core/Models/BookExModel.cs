using BymseBooks.DataLayer.Entity;

namespace BymseBooks.Core.Models;

public class BookExModel
{
    public BookModel Book { get; init; }
    
    public BookmarkModel[] Bookmarks { get; init; }
    
    public string? Url { get; init; }
}