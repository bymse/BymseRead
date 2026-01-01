using BymseRead.Core.Application.SingleBook;
using BymseRead.Core.Common.Models;

namespace BymseRead.Core.Application.BooksCollection;

public class BooksCollectionInfo
{
    public required IReadOnlyList<BookCollectionItem> ActiveBooks { get; init; }
    public required IReadOnlyList<BookCollectionItem> NewBooks { get; init; }
    public required IReadOnlyList<BookCollectionItem> TlDrBooks { get; init; }
    public required IReadOnlyList<BookCollectionItem> ArchivedBooks { get; init; }
}

public class BookCollectionItem
{
    public required Guid BookId { get; init; }
    public required string Title { get; init; }
    public required Uri? CoverUrl { get; init; }
    public required Uri FileUrl { get; init; }
    public required int? CurrentPage { get; init; }
    public required BookmarkInfo? LastBookmark { get; init; }
    public required int? Pages { get; init; }
}