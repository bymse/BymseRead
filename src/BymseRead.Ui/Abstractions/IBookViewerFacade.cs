namespace BymseRead.Ui.Abstractions;

public delegate Task ViewerInitializedEventHandler(IBookViewerFacade sender, ViewerInitializedEventArgs e);

public class ViewerInitializedEventArgs
{
    public int TotalPages { get; init; }
}

public interface IBookViewerFacade : IBookPagesFacade
{
    event ViewerInitializedEventHandler ViewerInitialized;
}