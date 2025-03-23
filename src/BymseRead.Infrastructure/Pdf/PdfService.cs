using BymseRead.Core.Common;
using BymseRead.Core.Services.Pdf;
using ImageMagick;
using UglyToad.PdfPig;

namespace BymseRead.Infrastructure.Pdf;

[AutoRegistration]
internal class PdfService : IPdfService
{
    public Task<int> GetPagesCount(PdfFileArgs args)
    {
        using var document = PdfDocument.Open(args.TempFilePath);
        return Task.FromResult(document.NumberOfPages);
    }

    public async Task<PageImageInfo> GetFirstPageAsImage(PdfFileArgs args)
    {
        var settings = new MagickReadSettings { Density = new Density(300), FrameIndex = 0, FrameCount = 1 };

        using var images = new MagickImageCollection();
        await images.ReadAsync(args.TempFilePath, settings);
        var firstPage = images.Single();

        var memoryStream = new MemoryStream();
        await firstPage.WriteAsync(memoryStream, MagickFormat.Png);

        var name = Path.GetFileNameWithoutExtension(args.FileName);
        return new PageImageInfo(memoryStream, $"{name}.png", memoryStream.Length);
    }
}