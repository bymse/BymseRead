using BymseRead.Core.Common;
using BymseRead.Core.Services.Files;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Core;

public static class CoreConfiguration
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddAutoRegistrations(typeof(CoreConfiguration).Assembly);

        services
            .AddOptions<FilesSettings>()
            .BindConfiguration(FilesSettings.Path);

        return services;
    }
}