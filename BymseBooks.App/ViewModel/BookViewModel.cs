using BymseBooks.DataLayer.Entity;
using CommunityToolkit.Mvvm.Input;

namespace BymseBooks.App.ViewModel;

public partial class BookViewModel
{
    public int BookId { get; init; }
    public string Title { get; init; }
    public string Author { get; init; }
    public IReadOnlyList<string> Tags { get; init; }
    public BookState BookState { get; init; }
    public int? Percents { get; init; }

    [RelayCommand]
    public async Task GoToBookPage()
    {
        await Shell.Current.GoToAsync($"book?bookId={BookId}");
    }
}