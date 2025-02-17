using BymseRead.Core.Application.BooksQueue;

namespace BymseRead.Service.Workers;

public class BooksQueueWorker(IServiceProvider sp) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested)
        {
            using var scope = sp.CreateScope();
            var queueProcessor = scope.ServiceProvider.GetRequiredService<BooksQueueProcessor>();
            
            var wasProcessed = await queueProcessor.ProcessNext();
            if (!wasProcessed)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}