namespace BymseRead.Core.Entities;

public record UserId(Guid Value);

public class User(string idp, string idpUserId)
{
    public UserId UserId { get; init; } = new(Guid.NewGuid());

    public string Idp { get; private init; } = idp;
    public string IdpUserId { get; private init; } = idpUserId;
}