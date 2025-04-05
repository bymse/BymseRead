using Microsoft.Extensions.Diagnostics.HealthChecks;
using BymseRead.Infrastructure.Database;
using BymseRead.Infrastructure.Files;

namespace BymseRead.Service.HealthChecks;

public class RemoteServicesHealthCheck(DataSourceProvider dataSourceProvider, S3FilesStorageService s3FilesStorageService) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = await dataSourceProvider.Get().OpenConnectionAsync(cancellationToken);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                return HealthCheckResult.Unhealthy("Database is not available");
            }

            if (!await s3FilesStorageService.IsBucketAvailable(cancellationToken))
            {
                return HealthCheckResult.Unhealthy("S3 is not available");
            }

            return HealthCheckResult.Healthy("All remote services are available");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("An error occurred while checking remote services", ex);
        }
    }
}