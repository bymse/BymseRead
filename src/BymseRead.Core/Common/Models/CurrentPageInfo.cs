namespace BymseRead.Core.Common.Models;

public class CurrentPageInfo
{
    public required int Page { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}