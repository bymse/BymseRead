namespace BymseRead.Ui.Abstractions;

public delegate Task PageChangeEventHandler(object sender, PageChangeEventArgs e);

public class PageChangeEventArgs
{
    public int NewPage { get; init; }
}

public interface IBookPagesFacade
{
    event PageChangeEventHandler PageChanged;
    Task SetPageAsync(int page);
}