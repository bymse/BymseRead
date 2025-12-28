namespace BymseRead.Infrastructure.Database.Entities;

public class DataProtectionKey
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string FriendlyName { get; init; }

    public required string Xml { get; init; }

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}