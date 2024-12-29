using BymseRead.Core;
using BymseRead.Core.Common;
using BymseRead.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddSingleton<DataSourceProvider>()
            .AddScoped<ConnectionFactory>()
            .AddCore()
            .AddAutoRegistrations(typeof(InfrastructureConfiguration).Assembly);
    }
}