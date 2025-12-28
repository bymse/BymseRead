using BymseRead.Core.Services.Books;

namespace BymseRead.Core.Application.SingleBook;

public class BookInfo
{
    public required Guid BookId { get; init; }
    public required string Title { get; init; }
    public required int? Pages { get; init; }
    public required FileInfo BookFile { get; init; }
    public required Uri? CoverUrl { get; init; }
    public required BookStatus Status { get; init; }

    public required int? CurrentPage { get; init; }
    public required BookmarkInfo? LastBookmark { get; init; }
}

public class FileInfo
{
    public required Uri FileUrl { get; init; }
    public required string Name { get; init; }
}

public class BookmarkInfo
{
    public required int Page { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}