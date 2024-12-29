namespace BymseRead.Core.Application.BooksCollection;

public class BooksCollectionInfo
{
    public required IReadOnlyList<BookShortInfo> ActiveBooks { get; init; }
    public required IReadOnlyList<BookShortInfo> NewBooks { get; init; }
    public required IReadOnlyList<BookShortInfo> TlDrBooks { get; init; }
    public required IReadOnlyList<BookShortInfo> ArchivedBooks { get; init; }
}

public class BookShortInfo
{
    public required string Title { get; init; }
    public required string[] Tags { get; init; }
    public required int PercentageFinished { get; init; }
    public required Uri? CoverUrl { get; init; }
}