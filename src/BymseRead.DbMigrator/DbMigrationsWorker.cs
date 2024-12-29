using FluentMigrator.Runner;

namespace BymseRead.DbMigrations;

public class DbMigrationsWorker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();

        hostApplicationLifetime.StopApplication();
        return Task.CompletedTask;
    }
}