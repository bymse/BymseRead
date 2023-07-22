using BymseRead.DataLayer.Database;
using BymseRead.DataLayer.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Core;

public static class CoreDependenciesRegisterer
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddDbContext<BooksDbContext>();
        services.AddTransient<IBookRepository, BookRepository>();
        services.AddTransient<ITagsRepository, TagsRepository>();
        services.AddTransient<IBookmarksRepository, BookmarksRepository>();

        services.AddTransient<BooksService>();
        services.AddTransient<BookmarksService>();
        services.AddTransient<TagsService>();
        
        return services;
    }
}