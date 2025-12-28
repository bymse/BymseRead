namespace BymseRead.Core.Application.UpdateBook;

public class UpdateBookRequest
{
    public required string Title { get; init; }

    public string? UploadedBookFileKey { get; init; }

    public string? UploadedCoverFileKey { get; init; }

    public bool RemoveCover { get; init; }
}