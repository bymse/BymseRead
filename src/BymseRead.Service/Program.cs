using BymseRead.Infrastructure;
using BymseRead.Infrastructure.Database.DataProtection;
using BymseRead.Service;
using BymseRead.Service.Auth;
using BymseRead.Service.Errors;
using BymseRead.Service.Swagger;
using BymseRead.Service.Workers;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection().PersistKeysToDatabase();

builder
    .Services
    .AddInfrastructure()
    .AddApi()
    .AddAuthN(builder.Configuration, builder.Environment)
    .AddWebSwagger()
    .AddProblemDetails()
    .AddExceptionHandler<ExceptionToProblemDetailsHandler>();

builder.Services.AddHostedService<BooksQueueWorker>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseWebSwagger();
}

app.UseRouting();
app.UseAuth();
app.MapControllers();

app.Run();

public partial class Program;