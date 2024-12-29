using BymseRead.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Infrastructure;

public static class InfrastructureConfiguration
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<DataSourceProvider>();
        services.AddScoped<ConnectionFactory>();
    }
}