namespace BymseRead.Core.Application.UpdateBook;

public class UpdateBookRequest
{
    public required string Title { get; init; }

    public required string? UploadedBookFileKey { get; init; }

    public required string? UploadedCoverFileKey { get; init; }

    public bool RemoveCover { get; init; }
}