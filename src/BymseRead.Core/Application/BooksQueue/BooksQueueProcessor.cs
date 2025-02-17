using BymseRead.Core.Common;
using BymseRead.Core.Services.BooksQueue;
using Microsoft.Extensions.Logging;

namespace BymseRead.Core.Application.BooksQueue;

[AutoRegistration]
public class BooksQueueProcessor(IBooksQueueService booksQueueService, ILogger<BooksQueueProcessor> logger)
{
    public async Task<bool> ProcessNext()
    {
        var context = await booksQueueService.ProcessNext();
        if (context.BookId == null)
        {
            return false;
        }

        try
        {
            await context.OnCompleted();
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                "An error occurred while processing book queue item {Id}",
                context.BooksQueueItemId);

            await context.OnFailed();
        }

        return true;
    }
}