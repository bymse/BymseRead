using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using BymseRead.Core;
using BymseRead.Core.Common;
using BymseRead.Core.Entities;
using BymseRead.Infrastructure.Database;
using BymseRead.Infrastructure.Database.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;
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
            .AddS3()
            .AddSingleton<DataSourceProvider>()
            .AddScoped<ConnectionFactory>()
            .AddCore()
            .AddAutoRegistrations(typeof(InfrastructureConfiguration).Assembly);
    }
    
    private static IServiceCollection AddS3(this IServiceCollection services)
    {
        AWSConfigsS3.UseSignatureVersion4 = true;
        
        return services.AddSingleton<IAmazonS3>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = new Uri(configuration.GetConnectionString("BymseReadS3") ??
                                           throw new InvalidOperationException("Missing S3 connection string"));

            var credentials = connectionString.UserInfo.Split(':');

            return new AmazonS3Client(new BasicAWSCredentials(credentials[0], credentials[1]),
                new AmazonS3Config
                {
                    ServiceURL =
                        $"{connectionString.Scheme}://{connectionString.Host}{connectionString.PathAndQuery}",
                    ForcePathStyle = true,
                });
        });
    }
}