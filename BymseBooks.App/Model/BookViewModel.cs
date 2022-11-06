using BymseBooks.DataLayer.Entity;

namespace BymseBooks.App.Model;

public class BookViewModel
{
    public int BookId { get; init; }
    public string Title { get; init; }
    public string Author { get; init; }
    public IReadOnlyList<string> Tags { get; init; }
    public BookState BookState { get; init; }
    public int? Percents { get; init; }
}