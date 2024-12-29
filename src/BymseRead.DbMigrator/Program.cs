using BymseRead.DbMigrations;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<DbMigrationsWorker>();

var host = builder.Build();
host.Run();