namespace BymseRead.Tests.Actions;

public class UsersActions
{
    public Task<Guid> CreateUser()
    {
        return Task.FromResult(Guid.NewGuid());
    }
}