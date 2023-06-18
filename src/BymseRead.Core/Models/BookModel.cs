using BymseRead.DataLayer.Entity;

namespace BymseRead.Core.Models;

public class BookModel
{
    public int Id { get; init; }
    public string Title { get; set; }
    public string[] Tags { get; set; }
    public string Author { get; set; }
    public int? Percents { get; init; }
    public BookState State { get; set; }
    public int? TotalPages { get; set; }
    
    public string[] TagsWithHashes => Tags.Select(e => $"#{e}").ToArray();
}