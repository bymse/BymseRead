namespace BymseRead.Core.Application.AddLastPageBookmark;

public class AddLastPageBookmarkRequest
{
    public required int Page { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}