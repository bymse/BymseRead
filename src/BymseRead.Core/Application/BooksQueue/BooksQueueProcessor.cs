using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using BymseRead.Core.Services.BooksQueue;
using BymseRead.Core.Services.Files;
using BymseRead.Core.Services.Pdf;
using Microsoft.Extensions.Logging;

namespace BymseRead.Core.Application.BooksQueue;

[AutoRegistration]
public class BooksQueueProcessor(
    IBooksQueueService booksQueueService,
    IPdfService pdfService,
    IBooksRepository booksRepository,
    IBooksQueryRepository booksQueryRepository,
    PdfCoverSaver pdfCoverSaver,
    IFilesStorageService filesStorageService,
    ILogger<BooksQueueProcessor> logger
)
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
            await ProcessBook(context.BookId);
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

    private async Task ProcessBook(BookId bookId)
    {
        var book = await booksQueryRepository.FindBook(bookId, null);
        if (book == null)
        {
            throw new InvalidOperationException("Book for processing not found");
        }

        using var pdfBookArgs = await GetPdfFile(book.BookFile);

        var pages = await pdfService.GetPagesCount(pdfBookArgs);
        book.Book.Pages = pages;

        if (book.CoverFile == null)
        {
            var cover = await pdfCoverSaver.SaveCover(book.Book.OwnerUserId, pdfBookArgs);
            book.Book.BookCoverFileId = cover?.Id;
        }

        await booksRepository.Update(book.Book);
    }


    private async Task<PdfFileArgs> GetPdfFile(File bookFile)
    {
        await using var stream = await filesStorageService.Download(bookFile);
        var tempFilePath = $"{Path.GetTempFileName()}.pdf";

        await using (var fileStream = System.IO.File.Create(tempFilePath))
        {
            await stream.CopyToAsync(fileStream);
        }

        return new PdfFileArgs(tempFilePath, bookFile.Name);
    }
}