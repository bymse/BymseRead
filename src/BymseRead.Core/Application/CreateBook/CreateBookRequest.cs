namespace BymseRead.Core.Application.CreateBook;

public class CreateBookRequest
{
    public required string FileUploadKey { get; init; }
    public required string Title { get; init; }
}