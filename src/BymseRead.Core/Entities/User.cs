namespace BymseRead.Core.Entities;

public record UserId(Guid Value) : IEntityId;

public class User
{
    public UserId Id { get; init; } = new(Guid.NewGuid());

    public string Idp { get; private init; }
    public string IdpUserId { get; private init; }

    private User()
    {
        Idp = null!;
        IdpUserId = null!;
    }

    public static User Create(string idp, string idpUserId)
    {
        return new User { Idp = idp, IdpUserId = idpUserId, };
    }
}