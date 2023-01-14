using BymseBooks.DataLayer.Database;
using BymseBooks.DataLayer.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BymseBooks.Core;

public static class CoreDependenciesRegisterer
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddDbContext<BooksDbContext>();
        services.AddTransient<IBookRepository, BookRepository>();
        services.AddTransient<IBookmarksRepository, BookmarksRepository>();

        services.AddTransient<BooksService>();
        
        return services;
    }
}