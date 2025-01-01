using System.ComponentModel.DataAnnotations;
using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.Files;

namespace BymseRead.Core.Services.Books;

[AutoRegistration]
public class BookValidator(IFilesStorageService filesStorageService, FilesValidator filesValidator)
{
    public async Task<UploadedFileModel> Validate(UserId userId, string fileUploadKey, string bookTitle)
    {
        ValidateTitle(bookTitle);

        var uploadedFile = await filesStorageService.FindUploadedFile(userId, fileUploadKey);
        if (uploadedFile == null)
        {
            throw new ValidationException
            {
                ValidationResult =
                {
                    ErrorMessage = "File not found. Probably book was created earlier. Please, refresh the page",
                },
            };
        }

        filesValidator.ValidateBookFile(uploadedFile.FileName, uploadedFile.Size);

        return uploadedFile;
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ValidationException { ValidationResult = { ErrorMessage = "Title is required" }, };
        }

        if (title.Length > Book.MaxTitleLength)
        {
            throw new ValidationException
            {
                ValidationResult = { ErrorMessage = $"Title is too long. Max length is {Book.MaxTitleLength}" },
            };
        }

        if (title.Length < 3)
        {
            throw new ValidationException
            {
                ValidationResult = { ErrorMessage = "Title is too short. Min length is 3" },
            };
        }
    }
}