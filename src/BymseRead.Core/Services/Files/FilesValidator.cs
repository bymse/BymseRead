using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace BymseRead.Core.Services.Files;

public class FilesValidator(IOptions<FilesSettings> settings)
{
    public void Validate(string fileName, long fileSize)
    {
        var extension = Path.GetExtension(fileName).TrimStart('.').ToLowerInvariant();
        
        if (!settings.Value.FileExtensionToMaxSize.TryGetValue(extension, out var maxSize))
        {
            throw new ValidationException
            {
                ValidationResult = { ErrorMessage = "File extension is not allowed." },
            };
        }
        
        if (fileSize > maxSize)
        {
            throw new ValidationException
            {
                ValidationResult = { ErrorMessage = "File size is too large" },
            };
        }
    }
}