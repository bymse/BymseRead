using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using BymseRead.Core.Services.Files;

namespace BymseRead.Core.Application.DeleteBook;

[AutoRegistration]
public class DeleteBookHandler(
    IBooksRepository booksRepository,
    IFilesRepository filesRepository,
    IBooksQueryRepository booksQueryRepository,
    IFilesStorageService filesStorageService
)
{
    public async Task Handle(UserId userId, BookId bookId)
    {
        var model = await booksQueryRepository.FindBook(bookId, userId);
        if (model is null)
        {
            return;
        }

        await booksRepository.Delete(model.Book);

        await filesRepository.Delete(model.BookFile);
        await filesStorageService.Delete(userId, model.BookFile);

        if (model.CoverFile != null)
        {
            await filesRepository.Delete(model.CoverFile);
            await filesStorageService.Delete(userId, model.CoverFile);
        }
    }
}