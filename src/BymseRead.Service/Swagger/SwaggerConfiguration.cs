using BymseRead.Service.WebApi;
using Microsoft.OpenApi;

namespace BymseRead.Service.Swagger;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddWebSwagger(this IServiceCollection services)
    {
        return services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(e =>
            {
                e.SupportNonNullableReferenceTypes();
                e.NonNullableReferenceTypesAsRequired();
                e.CustomOperationIds(r => r.ActionDescriptor.RouteValues["action"]);
                e.SwaggerDoc(WebApiController.DocumentName,
                    new OpenApiInfo { Title = WebApiController.DocumentName, Version = "1", });

                e.AddOperationFilterInstance(new ProblemDetailsFilter());
            });
    }

    public static IApplicationBuilder UseWebSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(e =>
        {
            e.SwaggerEndpoint($"/swagger/{WebApiController.DocumentName}/swagger.json", WebApiController.DocumentName);
        });

        return app;
    }
}