using BymseRead.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BymseRead.Service.Swagger;

public class ProblemDetailsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses?.Add("400",
            new OpenApiResponse
            {
                Description = "Bad Request",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails),
                            context.SchemaRepository),
                    },
                },
            });

        operation.Responses?.Add("401",
            new OpenApiResponse
            {
                Description = "Unauthorized",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(RedirectProblemDetails),
                            context.SchemaRepository),
                    },
                },
            });
    }
}