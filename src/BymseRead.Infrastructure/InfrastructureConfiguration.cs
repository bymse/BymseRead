using BymseRead.Core;
using BymseRead.Core.Common;
using BymseRead.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Infrastructure;

public static class InfrastructureConfiguration
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddSingleton<DataSourceProvider>()
            .AddScoped<ConnectionFactory>()
            .AddCore()
            .AddAutoRegistrations(typeof(InfrastructureConfiguration).Assembly);
    }
}