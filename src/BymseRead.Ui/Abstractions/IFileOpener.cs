namespace BymseRead.Ui.Abstractions;

public interface IFileOpener
{
    Task OpenFileAsync(string path);
}