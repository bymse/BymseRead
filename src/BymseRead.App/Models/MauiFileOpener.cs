using BymseRead.Ui.Abstractions;

namespace BymseRead.App.Models;

public class MauiFileOpener : IFileOpener
{
    public async Task OpenFileAsync(string path)
    {
        await Launcher.Default.OpenAsync(path);
    }
}