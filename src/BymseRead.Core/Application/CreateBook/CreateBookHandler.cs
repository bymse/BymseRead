using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using BymseRead.Core.Services.Books;
using BymseRead.Core.Services.BooksQueue;
using BymseRead.Core.Services.Files;

namespace BymseRead.Core.Application.CreateBook;

[AutoRegistration]
public class CreateBookHandler(
    IFilesStorageService filesStorageService,
    IBooksRepository booksRepository,
    BookValidator bookValidator,
    IFilesRepository filesRepository,
    IBooksQueueService booksQueueService
)
{
    public async Task<CreatedBookResult> Handle(UserId userId, CreateBookRequest request)
    {
        var uploadedFile = await bookValidator.Validate(userId, request.FileUploadKey, request.Title);

        var file = await filesStorageService.MakePermanent(userId, uploadedFile);
        await filesRepository.Add(file);

        var book = Book.Create(request.Title, file.Id, userId);
        await booksRepository.Add(book);

        await booksQueueService.Enqueue(book.Id);

        return new CreatedBookResult { BookId = book.Id.Value, };
    }
}