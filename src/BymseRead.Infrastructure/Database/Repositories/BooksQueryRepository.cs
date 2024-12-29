using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using Dapper;

namespace BymseRead.Infrastructure.Database.Repositories;

[AutoRegistration]
internal class BooksQueryRepository(ConnectionFactory connectionFactory) : IBooksQueryRepository
{
    public async Task<IEnumerable<UserBookModel>> GetUserBooks(UserId userId, string? search)
    {
        var connection = await connectionFactory.Get();
        return await connection.QueryAsync<UserBookModel>("", new { userId, search });
    }
}