using System.ComponentModel.DataAnnotations;
using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Services.Files;

namespace BymseRead.Core.Services.Books;

public record UploadedFiles(UploadedFileModel? BookFile, UploadedFileModel? CoverFile);

[AutoRegistration]
public class BookValidator(IFilesStorageService filesStorageService, FilesValidator filesValidator)
{
    public async Task<UploadedFileModel> Validate(UserId userId, string fileUploadKey, string bookTitle)
    {
        ValidateTitle(bookTitle);

        var uploadedFile = await filesStorageService.FindUploadedFile(userId, fileUploadKey);
        if (uploadedFile == null)
        {
            ValidationError.Throw("File not found. Probably book was created earlier. Please, refresh the page");
        }

        filesValidator.ValidateBookFile(uploadedFile.FileName, uploadedFile.Size);

        return uploadedFile;
    }

    public async Task<UploadedFiles> Validate(
        UserId userId,
        string? fileUploadKey,
        string? coverUploadKey,
        string bookTitle
    )
    {
        ValidateTitle(bookTitle);

        UploadedFileModel? bookFile = null;
        UploadedFileModel? coverFile = null;
        if (fileUploadKey != null)
        {
            bookFile = await filesStorageService.FindUploadedFile(userId, fileUploadKey);
            if (bookFile == null)
            {
                ValidationError.Throw("File not found. Probably book was updated earlier. Please, refresh the page");
            }

            filesValidator.ValidateBookFile(bookFile.FileName, bookFile.Size);
        }

        if (coverUploadKey != null)
        {
            coverFile = await filesStorageService.FindUploadedFile(userId, coverUploadKey);
            if (coverFile == null)
            {
                ValidationError.Throw(
                    "Cover file not found. Probably book was updated earlier. Please, refresh the page");
            }

            filesValidator.ValidateCoverFile(coverFile.FileName, coverFile.Size);
        }

        return new UploadedFiles(bookFile, coverFile);
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            ValidationError.Throw("Title is required");
        }

        if (title.Length > Book.MaxTitleLength)
        {
            ValidationError.Throw($"Title is too long. Max length is {Book.MaxTitleLength}");
        }

        if (title.Length < 3)
        {
            ValidationError.Throw("Title is too short. Min length is 3");
        }
    }
}