namespace BymseRead.Core.Entities;

public record BooksQueueItemId(Guid Value) : IEntityId;

public class BooksQueueItem
{
    public required BooksQueueItemId Id { get; init; } = new(Guid.NewGuid());
    
    public required BookId BookId { get; init; }
    
    public required BookQueueItemStatus Status { get; set; }
    
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public enum BookQueueItemStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
}