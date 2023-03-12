#nullable enable
using BymseBooks.Ui.Abstractions;

namespace BymseBooks.App.Models;

public class MauiFilePickHandler : IFilePickHandler
{
    public async Task<string?> HandleAsync()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Pdf
        });

        return result?.FullPath;
    }
}