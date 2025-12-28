using BymseRead.Core.Entities;

namespace BymseRead.Core.Repositories;

public interface IUsersRepository
{
    Task<UserId> Upsert(User user);
}