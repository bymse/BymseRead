using System.Data.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.BooksQueue;
using BymseRead.Infrastructure.Database.Repositories;
using Microsoft.Extensions.Logging;

namespace BymseRead.Infrastructure.BooksQueue;

internal class BookQueueItemContext : IBookQueueItemContext
{
    public static readonly BookQueueItemContext NothingToProcess = new();

    private readonly BooksQueueItem[] items;
    private readonly BooksQueueRepository repository;
    private readonly DbTransaction transaction;
    private readonly ILogger logger;

    public BookId? BookId { get; }

    public BookQueueItemContext(
        BooksQueueItem[] items,
        BooksQueueRepository repository,
        DbTransaction transaction,
        ILogger logger
    )
    {
        this.items = items;
        this.repository = repository;
        this.transaction = transaction;
        this.logger = logger;
        BookId = items
            .Select(e => e.BookId)
            .Distinct()
            .Single();
    }

    private BookQueueItemContext()
    {
        logger = null!;
        items = null!;
        repository = null!;
        transaction = null!;
        BookId = null;
    }

    public async Task OnCompleted()
    {
        foreach (var item in items)
        {
            item.Completed();
        }

        var firstItem = items.First();
        var ids = items
            .Select(e => e.Id)
            .ToArray();

        await repository.Update(ids, firstItem.Status, firstItem.UpdatedAt);
        await transaction.CommitAsync();
    }

    public async Task OnFailed(Exception exception)
    {
        var ids = items
            .Select(e => e.Id)
            .ToArray();

        logger.LogError(exception,
            "An error occurred while processing book queue items: {Ids}",
            string.Join(",", ids.AsEnumerable()));

        foreach (var item in items)
        {
            item.Failed();
        }

        var firstItem = items.First();
        await repository.Update(ids, firstItem.Status, firstItem.UpdatedAt);
        await transaction.CommitAsync();
    }
}