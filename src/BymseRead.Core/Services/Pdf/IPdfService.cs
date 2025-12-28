namespace BymseRead.Core.Services.Pdf;

public interface IPdfService
{
    Task<PageImageInfo> GetFirstPageAsImage(PdfFileArgs args);
    Task<int> GetPagesCount(PdfFileArgs args);
}

public record PdfFileArgs(string TempFilePath, string FileName) : IDisposable
{
    public void Dispose()
    {
        System.IO.File.Delete(TempFilePath);
    }
}

public record PageImageInfo(Stream ImageStream, string Name, long Size) : IDisposable, IAsyncDisposable
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