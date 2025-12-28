using System.ComponentModel.DataAnnotations;
using BymseRead.Core.Common;
using Microsoft.Extensions.Options;

namespace BymseRead.Core.Services.Files;

[AutoRegistration]
public class FilesValidator(IOptions<FilesSettings> settings)
{
    public void ValidateBookFile(string fileName, long fileSize)
    {
        Validate(fileName, fileSize, settings.Value.BooksFiles);
    }

    public void ValidateCoverFile(string fileName, long fileSize)
    {
        Validate(fileName, fileSize, settings.Value.CoverFiles);
    }

    public void Validate(string fileName, long fileSize)
    {
        Validate(fileName, fileSize, settings.Value.BooksFiles.Concat(settings.Value.CoverFiles));
    }

    private static void Validate(string fileName, long fileSize, IEnumerable<FileTypeSettings> allowedTypesSettings)
    {
        var extension = Path
            .GetExtension(fileName)
            .TrimStart('.')
            .ToLowerInvariant();

        var typeSettings = allowedTypesSettings.FirstOrDefault(x => x.Extension == extension);

        if (typeSettings == null)
        {
            throw new ValidationException { ValidationResult = { ErrorMessage = "File extension is not allowed." }, };
        }

        if (fileSize > typeSettings.MaxSize)
        {
            throw new ValidationException { ValidationResult = { ErrorMessage = "File size is too large" }, };
        }
    }
}