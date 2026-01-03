namespace BymseRead.Core.Application.UpdateCurrentPage;

public class UpdateCurrentPageRequest
{
    public required int Page { get; init; }
    public required DateTimeOffset ChangedAt { get; init; }
}