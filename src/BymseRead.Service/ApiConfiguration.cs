using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service;

public static class ApiConfiguration
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services
            .Configure<MvcOptions>(e =>
            {
                e.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = false;
            })
            .AddRouting(e =>
            {
                e.LowercaseUrls = true;
                e.LowercaseQueryStrings = true;
            })
            .AddControllers()
            .AddJsonOptions(e =>
            {
                e.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                e.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }
}