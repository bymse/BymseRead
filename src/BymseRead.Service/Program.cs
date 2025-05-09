using BymseRead.Infrastructure;
using BymseRead.Infrastructure.Database.DataProtection;
using BymseRead.Service;
using BymseRead.Service.Auth;
using BymseRead.Service.Errors;
using BymseRead.Service.HealthChecks;
using BymseRead.Service.Swagger;
using BymseRead.Service.Workers;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
});

builder.Services.AddDataProtection().PersistKeysToDatabase();

builder
    .Services
    .AddMemoryCache()
    .AddSingleton<RemoteServicesHealthCheck>()
    .AddInfrastructure()
    .AddApi()
    .AddAuthN(builder.Configuration, builder.Environment)
    .AddWebSwagger()
    .AddProblemDetails()
    .AddExceptionHandler<ExceptionToProblemDetailsHandler>();

builder.Services.AddHostedService<BooksQueueWorker>();

builder.Services
    .AddHealthChecks()
    .AddCheck<RemoteServicesHealthCheck>("remote_services_health_check", timeout: TimeSpan.FromSeconds(10));

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseWebSwagger();
}

app.UseRouting();
app.UseAuth();
app.MapControllers();
app.UseForwardedHeaders();

app.MapHealthChecks("/healthcheck");

app.Run();

public partial class Program;