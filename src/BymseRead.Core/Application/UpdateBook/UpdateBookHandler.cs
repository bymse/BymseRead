using System.ComponentModel.DataAnnotations;
using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using BymseRead.Core.Services;
using BymseRead.Core.Services.Books;
using BymseRead.Core.Services.BooksQueue;
using BymseRead.Core.Services.Files;

namespace BymseRead.Core.Application.UpdateBook;

[AutoRegistration]
public class UpdateBookHandler(
    IFilesStorageService filesStorageService,
    IBooksRepository booksRepository,
    IBooksQueryRepository booksQueryRepository,
    BookValidator bookValidator,
    IFilesRepository filesRepository,
    IBooksQueueService booksQueueService
)
{
    public async Task Handle(UserId userId, BookId bookId, UpdateBookRequest request)
    {
        var model = await booksQueryRepository.FindBook(bookId, userId);
        if (model == null)
        {
            ValidationError.Throw("Book not found");
        }

        var (uploadedBook, uploadedCover) = await bookValidator.Validate(userId,
            request.UploadedBookFileKey,
            request.UploadedCoverFileKey,
            request.Title);

        model.Book.Title = request.Title;

        if (uploadedBook != null)
        {
            var file = await filesStorageService.MakePermanent(userId, uploadedBook);
            await filesRepository.Add(file);
            model.Book.BookFileId = file.Id;
        }

        if (uploadedCover != null)
        {
            var file = await filesStorageService.MakePermanent(userId, uploadedCover);
            await filesRepository.Add(file);
            model.Book.BookCoverFileId = file.Id;
        }
        else if (request.RemoveCover)
        {
            model.Book.BookCoverFileId = null;
        }

        await booksRepository.Update(model.Book);

        if (uploadedBook != null)
        {
            await filesStorageService.Delete(userId, model.BookFile);
            await filesRepository.Delete(model.BookFile);
            await booksQueueService.Enqueue(model.Book.Id);
        }

        if (model.CoverFile != null && (uploadedCover != null || request.RemoveCover))
        {
            await filesStorageService.Delete(userId, model.CoverFile);
            await filesRepository.Delete(model.CoverFile);
        }
    }
}