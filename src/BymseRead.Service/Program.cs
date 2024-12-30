using System.Text.Json;
using System.Text.Json.Serialization;
using BymseRead.Infrastructure;
using BymseRead.Service.WebApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddInfrastructure()
    .AddRouting(e =>
    {
        e.LowercaseUrls = true;
        e.LowercaseQueryStrings = true;
    })
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(e =>
    {
        e.CustomOperationIds(r => r.ActionDescriptor.RouteValues["action"]);
        e.SwaggerDoc(WebApiController.DocumentName,
            new OpenApiInfo { Title = WebApiController.DocumentName, Version = "1", });
    })
    .AddProblemDetails()
    .AddExceptionHandler(e => {})
    .AddControllers()
    .AddJsonOptions(e =>
    {
        e.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        e.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(e =>
    {
        e.SwaggerEndpoint($"/swagger/{WebApiController.DocumentName}/swagger.json", WebApiController.DocumentName);
    });
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();