namespace BymseBooks.Ui.Abstractions;

public interface IFilePickHandler
{
    Task<string?> HandleAsync();
}