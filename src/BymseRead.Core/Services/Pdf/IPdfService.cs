namespace BymseRead.Core.Services.Pdf;

public interface IPdfService
{
    Task<PageImageInfo> GetFirstPageAsImage(PdfFileArgs args);
    Task<int> GetPagesCount(PdfFileArgs args);
}

public record PdfFileArgs(string TempFilePath, string FileName);

public record PageImageInfo(Stream ImageStream, string Name) : IDisposable, IAsyncDisposable
{
    public void Dispose()
    {
        ImageStream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await ImageStream.DisposeAsync();
    }
}