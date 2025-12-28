using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Core.Repositories;
using BymseRead.Core.Services.Pdf;
using Microsoft.Extensions.Logging;

namespace BymseRead.Core.Services.Files;

[AutoRegistration]
public class PdfCoverSaver(
    IPdfService pdfService,
    IFilesStorageService filesStorageService,
    FilesValidator filesValidator,
    IFilesRepository filesRepository,
    ILogger<PdfCoverSaver> logger
)
{
    public async Task<File?> SaveCover(UserId userId, PdfFileArgs args)
    {
        try
        {
            await using var image = await pdfService.GetFirstPageAsImage(args);
            filesValidator.ValidateCoverFile(image.Name, image.Size);

            var coverFile = await filesStorageService.Upload(userId, image.ImageStream, image.Name);
            await filesRepository.Add(coverFile);
            return coverFile;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while extracting cover from PDF");
            return null;
        }
    }
}