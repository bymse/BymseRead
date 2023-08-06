using BymseRead.Core;
using BymseRead.DataLayer.Database;
using BymseRead.DataLayer.Repository;
using BymseRead.Ui.Abstractions;
using BymseRead.App.Models;
using BymseRead.Ui.Models.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.LifecycleEvents;

namespace BymseRead.App
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
                .AddScoped<BookPageState>()
                ;

            var app = builder.Build();
            app.Services.GetService<BooksDbContext>().Database.Migrate();

            return app;
        }
    }
}