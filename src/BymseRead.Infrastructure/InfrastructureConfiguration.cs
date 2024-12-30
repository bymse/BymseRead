using BymseRead.Core;
using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Infrastructure.Database;
using BymseRead.Infrastructure.Database.Dapper;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        typeof(IEntityId)
            .Assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t
                .GetInterfaces()
                .Contains(typeof(IEntityId)))
            .ToList()
            .ForEach(t => { SqlMapper.AddTypeHandler(t, new EntityIdTypeHandler(t)); });

        SqlMapper.TypeMapProvider = type => new ThrowOnMissingMap(type); 
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        return services
            .AddSingleton<DataSourceProvider>()
            .AddScoped<ConnectionFactory>()
            .AddCore()
            .AddAutoRegistrations(typeof(InfrastructureConfiguration).Assembly);
    }
}