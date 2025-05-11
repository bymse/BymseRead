using BymseRead.Core.Repositories;
using BymseRead.Core.Services.BooksQueue;

namespace BymseRead.DbMigrator;

public class ReprocessBooksWorker(IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService 
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IBooksQueueService>();
        var repository = scope.ServiceProvider.GetRequiredService<IBooksRepository>();
        var fileRepository = scope.ServiceProvider.GetRequiredService<IFilesRepository>();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IBooksQueryRepository>();

        var books = await queryRepository.GetBooks();
        foreach (var userBookModel in books)
        {
            if (userBookModel.CoverFile != null)
            {
                userBookModel.Book.BookCoverFileId = null;
                await repository.Update(userBookModel.Book);
                await fileRepository.Delete(userBookModel.CoverFile);
                await service.Enqueue(userBookModel.Book.Id);
            }
        }
        
        hostApplicationLifetime.StopApplication();
    }
}