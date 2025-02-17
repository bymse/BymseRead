using System.Data.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.BooksQueue;
using BymseRead.Infrastructure.Database.Repositories;

namespace BymseRead.Infrastructure.BooksQueue;

internal class BookQueueItemContext : IBookQueueItemContext
{
    public static readonly BookQueueItemContext NothingToProcess = new();

    private readonly BooksQueueItem? _item;
    private readonly BooksQueueRepository _repository;
    private readonly DbTransaction _transaction;

    public BookId? BookId => _item?.BookId;
    public BooksQueueItemId? BooksQueueItemId => _item?.Id;

    public BookQueueItemContext(BooksQueueItem item, BooksQueueRepository repository, DbTransaction transaction)
    {
        _item = item;
        _repository = repository;
        _transaction = transaction;
    }

    private BookQueueItemContext()
    {
        _item = null;
        _repository = null!;
        _transaction = null!;
    }

    public async Task OnCompleted()
    {
        _item!.Completed();
        await _repository.Update(_item);
        await _transaction.CommitAsync();
    }

    public async Task OnFailed()
    {
        _item!.Failed();
        await _repository.Update(_item);
        await _transaction.CommitAsync();
    }
}