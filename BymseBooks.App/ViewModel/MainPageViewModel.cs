using System.Collections.ObjectModel;
using BymseBooks.App.Model;
using BymseBooks.App.Service;
using BymseBooks.DataLayer.Entity;
using CommunityToolkit.Mvvm.Input;

namespace BymseBooks.App.ViewModel;

public partial class MainPageViewModel : BaseViewModel
{
    private readonly BooksService booksService;

    public MainPageViewModel(BooksService booksService)
    {
        this.booksService = booksService;
        GetBooks();
    }

    public ObservableCollection<BookViewModel> Books { get; } = new();

    [RelayCommand]
    private void GetBooks()
    {
        var books = booksService.GetBooks(BookState.Active, 10, 0);
        if (books.Count == 0) return;
        
        Books.Clear();
        foreach (var bookViewModel in books)
        {
            Books.Add(bookViewModel);
        }
    }
}