using System.Data.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.BooksQueue;
using BymseRead.Infrastructure.Database.Repositories;
using Microsoft.Extensions.Logging;

namespace BymseRead.Infrastructure.BooksQueue;

internal class BookQueueItemContext : IBookQueueItemContext
{
    public static readonly BookQueueItemContext NothingToProcess = new();

    private readonly BooksQueueItem[] _items;
    private readonly BooksQueueRepository _repository;
    private readonly DbTransaction _transaction;
    private readonly ILogger _logger;


    public BookId? BookId { get; }

    public BookQueueItemContext(
        BooksQueueItem[] items,
        BooksQueueRepository repository,
        DbTransaction transaction,
        ILogger logger
    )
    {
        _items = items;
        _repository = repository;
        _transaction = transaction;
        _logger = logger;
        BookId = items
            .Select(e => e.BookId)
            .Distinct()
            .Single();
    }

    private BookQueueItemContext()
    {
        _logger = null!;
        _items = null!;
        _repository = null!;
        _transaction = null!;
        BookId = null;
    }

    public async Task OnCompleted()
    {
        foreach (var item in _items)
        {
            item.Completed();
        }

        var firstItem = _items.First();
        var ids = _items
            .Select(e => e.Id)
            .ToArray();

        await _repository.Update(ids, firstItem.Status, firstItem.UpdatedAt);
        await _transaction.CommitAsync();
    }

    public async Task OnFailed(Exception exception)
    {
        var ids = _items
            .Select(e => e.Id)
            .ToArray();

        _logger.LogError(exception,
            "An error occurred while processing book queue items: {Ids}",
            string.Join(",", ids.AsEnumerable()));

        foreach (var item in _items)
        {
            item.Failed();
        }

        var firstItem = _items.First();
        await _repository.Update(ids, firstItem.Status, firstItem.UpdatedAt);
        await _transaction.CommitAsync();
    }
}