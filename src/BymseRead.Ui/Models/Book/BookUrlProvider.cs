using System.Net;
using BymseRead.DataLayer.Helpers;

namespace BymseRead.Ui.Models.Book;

public static class BookUrlProvider
{
    public static string GetUrl(string? url)
    {
        return url.IsNullOrEmpty()
            ? ""
            : $"/user-file/{WebUtility.UrlEncode(new Uri(url!).AbsolutePath)}";
    }
}