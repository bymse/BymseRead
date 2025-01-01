using BymseRead.Infrastructure;
using BymseRead.Service;
using BymseRead.Service.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddInfrastructure()
    .AddApi()
    .AddWebSwagger()
    .AddProblemDetails()
    .AddExceptionHandler(e => {});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseWebSwagger();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();