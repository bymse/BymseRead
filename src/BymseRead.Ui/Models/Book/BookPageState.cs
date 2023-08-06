using BymseRead.Core.Models;

namespace BymseRead.Ui.Models.Book;

public class BookPageState : IDisposable
{
    public BookModel Book { get; private set; }
    public bool ShowBookmarks { get; private set; }
    public bool ShowEditForm { get; set; }
    public int CurrentPage { get; set; }
    public string BookLoadUrl => BookUrlProvider.GetUrl(Book.Url);
    public string Title => $"{Book.Title} by {Book.Author}";
    
    public void ToggleBookmarks()
    {
        ShowBookmarks = !ShowBookmarks;
    }

    public void OpenEditForm()
    {
        ShowEditForm = true;
    }

    public void SetBook(BookModel book)
    {
        Book = book;
        CurrentPage = book.LastViewedPage ?? 1;
    }

    public void Dispose()
    {
        
    }
}