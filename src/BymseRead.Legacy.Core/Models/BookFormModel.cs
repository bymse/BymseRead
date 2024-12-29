namespace BymseRead.Legacy.Core.Models;

public class BookFormModel
{
    public string Title { get; set; }
    public string? Author { get; set; }
    public IList<string> Tags { get; set; }
    public string? Url { get; set; }
    public int? BookId { get; set; }
}