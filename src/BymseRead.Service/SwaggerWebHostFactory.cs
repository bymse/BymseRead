using BymseRead.Service.Swagger;
using JetBrains.Annotations;
using Microsoft.AspNetCore;

namespace BymseRead.Service;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class SwaggerWebHostFactory
{
    public static IWebHost CreateWebHost()
    {
        return WebHost
            .CreateDefaultBuilder([])
            .ConfigureServices(services =>
            {
                services
                    .AddWebSwagger()
                    .AddApi();
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseWebSwagger();
                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            })
            .Build();
    }
}