namespace BymseRead.Ui.Abstractions;

public interface IFilePickHandler
{
    Task<string?> HandleAsync();
}