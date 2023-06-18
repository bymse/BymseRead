using BymseBooks.App.Models;
using BymseBooks.Core;
using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Repository;
using BymseBooks.Ui.Abstractions;
using Microsoft.EntityFrameworkCore;
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

            ConfigurationSetup.Configure(builder.Configuration);
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazorWebViewDeveloperTools();

            builder.Services
                .AddCoreServices()
                .AddSingleton<IFilePickHandler, MauiFilePickHandler>()
                ;

            var app = builder.Build();
            app.Services.GetService<BooksDbContext>().Database.Migrate();

            return app;
        }
    }
}