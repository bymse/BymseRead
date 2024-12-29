using FluentMigrator.Runner;

namespace BymseRead.DbMigrations;

public class DbMigrationsWorker(IServiceProvider serviceProvider) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
        
        return Task.CompletedTask;
    }
}