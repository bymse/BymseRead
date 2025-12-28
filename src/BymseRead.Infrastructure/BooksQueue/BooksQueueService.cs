using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.BooksQueue;
using BymseRead.Infrastructure.Database;
using BymseRead.Infrastructure.Database.Repositories;
using Microsoft.Extensions.Logging;

namespace BymseRead.Infrastructure.BooksQueue;

[AutoRegistration]
internal class BooksQueueService(BooksQueueRepository repository, ConnectionFactory connectionFactory, ILogger<BookQueueItemContext> logger)
    : IBooksQueueService
{
    public async Task<IBookQueueItemContext> ProcessNext()
    {
        var connection = await connectionFactory.Get();
        var transaction = await connection.BeginTransactionAsync();

        var items = await repository.GetNextItemsForUpdate();
        if (items.Length == 0)
        {
            await transaction.RollbackAsync();
            return BookQueueItemContext.NothingToProcess;
        }

        return new BookQueueItemContext(items, repository, transaction, logger);
    }

    public async Task Enqueue(BookId bookId)
    {
        var item = new BooksQueueItem { BookId = bookId, Status = BookQueueItemStatus.Pending, };
        await repository.Add(item);
    }
}