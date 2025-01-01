using System.Text.Json;
using System.Text.Json.Serialization;

namespace BymseRead.Service;

public static class ApiConfiguration
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services
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