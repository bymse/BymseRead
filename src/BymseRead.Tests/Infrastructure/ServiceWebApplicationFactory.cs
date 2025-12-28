using BymseRead.Core.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BymseRead.Tests.Infrastructure;

public class ServiceWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string TestHeaderAuthScheme = "TestHeaderAuth";

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(e =>
        {
            e.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "appsettings.Tests.json"));
        });

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureServices(r =>
        {
            r.AddAutoRegistrations(typeof(ServiceWebApplicationFactory).Assembly);
            r.AddTransient(_ => CreateDefaultClient());

            r
                .AddAuthentication(options => { options.DefaultScheme = TestHeaderAuthScheme; })
                .AddScheme<AuthenticationSchemeOptions, TestHeaderAuthHandler>(TestHeaderAuthScheme, _ => { });
        });
    }
}