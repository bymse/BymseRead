using Amazon.Runtime;
using Amazon.S3;
using BymseRead.Core;
using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Infrastructure.Database;
using BymseRead.Infrastructure.Database.Dapper;
using BymseRead.Infrastructure.Files;
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
            .ForEach(t =>
            {
                SqlMapper.AddTypeHandler(t, new ValueObjectTypeHandler(t));
                SqlMapper.AddTypeHandler(t.MakeArrayType(), new ValueObjectTypeHandler(t));
            });

        SqlMapper.TypeMapProvider = type => new ThrowOnMissingMap(type);
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services
            .AddMemoryCache()
            .AddS3()
            .AddSingleton<DataSourceProvider>()
            .AddScoped<ConnectionFactory>()
            .AddCore()
            .AddAutoRegistrations(typeof(InfrastructureConfiguration).Assembly);
    }

    private static IServiceCollection AddS3(this IServiceCollection services)
    {
        services.AddSingleton<S3ConfigurationHelper>();
        services
            .AddOptions<S3FilesStorageSettings>()
            .BindConfiguration(S3FilesStorageSettings.Path);

        return services.AddSingleton<IAmazonS3>(sp =>
        {
            var config = sp.GetRequiredService<S3ConfigurationHelper>().GetS3Config();

            return new AmazonS3Client(new BasicAWSCredentials(config.AccessKey, config.SecretKey),
                new AmazonS3Config { ServiceURL = config.ServiceUrl, ForcePathStyle = true, });
        });
    }
}