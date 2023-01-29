using BymseBooks.App.Models;
using BymseBooks.Core;
using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Repository;
using BymseBooks.Ui.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.LifecycleEvents;

namespace BymseBooks.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    
                    fonts.AddFont("Cormorant-Bold.ttf", "CormorantBold");
                    fonts.AddFont("Cormorant-Italic.ttf", "CormorantItalic");
                    fonts.AddFont("Cormorant-Regular.ttf", "CormorantRegular");
                })
                ;

            builder.Services.AddMauiBlazorWebView();
            builder.Configuration.AddJsonFile("appsettings.json");

#if DEBUG
            builder.Configuration.AddJsonFile("appsettings.Debug.json");
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services
                .AddCoreServices()
                .AddSingleton<IFilePickHandler, MauiFilePickHandler>()
                ;

            return builder.Build();
        }
    }
}