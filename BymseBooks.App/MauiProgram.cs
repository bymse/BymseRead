using BymseBooks.App.Service;
using BymseBooks.App.ViewModel;
using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Repository;
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
                })
                ;

            builder.Configuration.AddJsonFile("appsettings.json");

#if DEBUG
            builder.Configuration.AddJsonFile("appsettings.Debug.json");
#endif

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<IBookRepository, BookRepository>();
            builder.Services.AddTransient<BooksDbContext>();
            
            return builder.Build();
        }
    }
}