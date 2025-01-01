using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using Dapper;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class UsersRepository(ConnectionFactory connectionFactory) : IUsersRepository
{
    public async Task<UserId> Upsert(User user)
    {
        var connection = await connectionFactory.Get();
        const string sql = """
                           with modified as (
                               insert into users (id, idp, idp_user_id)
                                   values (@Id, @Idp, @IdpUserId)
                                   on conflict (idp, idp_user_id) do nothing
                                   returning id
                           )
                           select id from modified
                           union all
                           select id from users
                           where idp = @Idp and idp_user_id = @IdpUserId
                           """;

        return await connection.QuerySingleAsync<UserId>(sql, user);
    }
}