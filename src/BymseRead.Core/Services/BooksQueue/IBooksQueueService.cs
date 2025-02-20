using BymseRead.Core.Entities;

namespace BymseRead.Core.Services.BooksQueue;

public interface IBooksQueueService
{
    Task<IBookQueueItemContext> ProcessNext();
    Task Enqueue(BookId bookId);
}

public interface IBookQueueItemContext
{
    Task OnCompleted();
    Task OnFailed(Exception exception);
    BookId? BookId { get; }
}