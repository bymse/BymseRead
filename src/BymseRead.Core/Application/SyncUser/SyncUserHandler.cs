using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;

namespace BymseRead.Core.Application.SyncUser;

[AutoRegistration]
public class SyncUserHandler(IUsersRepository repository)
{
    public async Task<UserId> Handle(string idp, string idpUserId)
    {
        var user = User.Create(idp, idpUserId);
        var id = await repository.Upsert(user);
        return id;
    }
}