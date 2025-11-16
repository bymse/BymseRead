using System.Reflection;
using BymseRead.DbMigrator;
using BymseRead.Infrastructure;
using FluentMigrator.Runner;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHostedService<DbMigrationsWorker>();

builder
    .Services.AddFluentMigratorCore()
    .AddInfrastructure()
    .ConfigureRunner(r => r
        .AddPostgres()
        .WithGlobalConnectionString(builder.Configuration.GetConnectionString("BymseReadPostgres"))
        .ScanIn(Assembly.GetExecutingAssembly())
        .For.Migrations())
    .AddLogging(r => r.AddFluentMigratorConsole());

var host = builder.Build();
host.Run();