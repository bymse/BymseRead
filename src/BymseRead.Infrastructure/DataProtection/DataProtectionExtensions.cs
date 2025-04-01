using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace BymseRead.Infrastructure.Database.DataProtection;

public static class DataProtectionExtensions
{
    public static IDataProtectionBuilder PersistKeysToDatabase(this IDataProtectionBuilder builder)
    {
        builder.SetApplicationName("BymseRead");

        builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services =>
        {
            var loggerFactory = services.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
            return new ConfigureOptions<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new DatabaseXmlRepository(services, loggerFactory);
            });
        });

        return builder;
    }
}