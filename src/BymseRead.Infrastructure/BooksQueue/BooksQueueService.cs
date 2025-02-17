using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.BooksQueue;
using BymseRead.Infrastructure.Database;
using BymseRead.Infrastructure.Database.Repositories;

namespace BymseRead.Infrastructure.BooksQueue;

[AutoRegistration]
internal class BooksQueueService(BooksQueueRepository repository, ConnectionFactory connectionFactory)
    : IBooksQueueService
{
    public async Task<IBookQueueItemContext> ProcessNext()
    {
        var connection = await connectionFactory.Get();
        var transaction = await connection.BeginTransactionAsync();

        var item = await repository.GetNextItemForUpdate();
        if (item == null)
        {
            await transaction.RollbackAsync();
            return BookQueueItemContext.NothingToProcess;
        }

        return new BookQueueItemContext(item, repository, transaction);
    }

    public async Task Enqueue(BookId bookId)
    {
        var item = new BooksQueueItem { BookId = bookId, Status = BookQueueItemStatus.Pending, };
        await repository.Add(item);
    }
}