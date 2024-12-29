using BymseRead.Core.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Core;

public static class CoreConfiguration
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddAutoRegistrations(typeof(CoreConfiguration).Assembly);

        return services;
    }
}