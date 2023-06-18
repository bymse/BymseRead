using System.Net;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;

namespace BymseRead.App;

public class CustomBlazorWebView : BlazorWebView
{
    public override IFileProvider CreateFileProvider(string contentRootDir)
    {
        return new CustomFileProvider(new NullFileProvider());
    }
}

class CustomFileProvider : IFileProvider
{
    private readonly IFileProvider baseFileProvider;

    public CustomFileProvider(IFileProvider baseFileProvider)
    {
        this.baseFileProvider = baseFileProvider;
    }
    
    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return baseFileProvider.GetDirectoryContents(subpath);
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        if (subpath.StartsWith("user-file"))
        {
            var file = WebUtility.UrlDecode(WebUtility.UrlDecode(subpath.Split("/")[1]));
            return new PhysicalFileInfo(new FileInfo(file));
        }
        
        return baseFileProvider.GetFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return baseFileProvider.Watch(filter);
    }
}