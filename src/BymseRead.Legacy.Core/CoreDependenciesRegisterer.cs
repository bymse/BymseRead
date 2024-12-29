using BymseRead.Legacy.DataLayer.Database;
using BymseRead.Legacy.DataLayer.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Legacy.Core;

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