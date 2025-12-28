namespace BymseRead.Core.Entities;

public record BooksQueueItemId(Guid Value) : IEntityId;

public class BooksQueueItem
{
    public BooksQueueItemId Id { get; init; } = new(Guid.NewGuid());

    public required BookId BookId { get; init; }

    public required BookQueueItemStatus Status { get; set; }

    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public void Completed()
    {
        Status = BookQueueItemStatus.Completed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Failed()
    {
        Status = BookQueueItemStatus.Failed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}

public enum BookQueueItemStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
}