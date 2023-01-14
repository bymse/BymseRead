using BymseBooks.DataLayer.Entity;

namespace BymseBooks.Core.Models;

public class BookmarkModel
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public BookmarkType Type { get; init; }
    public DateTime Date { get; init; }
    public int Page { get; init; }
}