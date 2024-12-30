using System.Reflection;
using BymseRead.DbMigrations;
using FluentMigrator.Runner;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<DbMigrationsWorker>();

builder
    .Services.AddFluentMigratorCore()
    .ConfigureRunner(r => r
        .AddPostgres()
        .WithGlobalConnectionString(builder.Configuration.GetConnectionString("BymseReadPostgres"))
        .ScanIn(Assembly.GetExecutingAssembly())
        .For.Migrations())
    .AddLogging(r => r.AddFluentMigratorConsole());

var host = builder.Build();
host.Run();