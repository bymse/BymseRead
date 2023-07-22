using BymseRead.Ui.Abstractions;

namespace BymseRead.Ui.Pages.Book;

public class BookPdfViewerFacade : IBookViewerFacade, IDisposable
{
    private PdfViewer pdfViewer;

    public PdfViewer PdfViewerRef
    {
        set
        {
            pdfViewer = value;
            pdfViewer.PageChanged += PageChanged;
            pdfViewer.ViewerInitialized += ViewerInitialized;
        }
    }

    public Task SetPageAsync(int page)
    {
        return pdfViewer.SetPageAsync(page);
    }

    public event PageChangeEventHandler? PageChanged;
    public event ViewerInitializedEventHandler? ViewerInitialized;

    public void Dispose()
    {
        pdfViewer.Dispose();
    }
}