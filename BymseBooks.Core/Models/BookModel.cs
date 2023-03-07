using BymseBooks.DataLayer.Entity;

namespace BymseBooks.Core.Models;

public class BookModel
{
    public int Id { get; init; }
    public string Title { get; set; }
    public string[] Tags { get; set; }
    public string Author { get; set; }
    public int? Percents { get; init; }
    public BookState State { get; init; }
    public int? TotalPages { get; set; }
}