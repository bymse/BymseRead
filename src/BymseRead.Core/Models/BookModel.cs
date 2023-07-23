using BymseRead.DataLayer.Entity;

namespace BymseRead.Core.Models;

public class BookModel
{
    public int Id { get; init; }
    public string? Title { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string? Author { get; set; }
    public BookState State { get; set; }
    public int? TotalPages { get; set; }
    public int? LastViewedPage { get; init; }
    public string? Url { get; set; }
    public int? PercentageRead { get; init; }
    
    public string[] TagsWithHashes => Tags.Select(e => $"#{e}").ToArray();
}