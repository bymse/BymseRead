using System.Xml.Linq;
using BymseRead.Infrastructure.Database.Entities;
using BymseRead.Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BymseRead.Infrastructure.Database.DataProtection;

internal class DatabaseXmlRepository(IServiceProvider services, ILoggerFactory loggerFactory) : IXmlRepository
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<DatabaseXmlRepository>();

    public IReadOnlyCollection<XElement> GetAllElements()
    {
        using var scope = services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<DataProtectionKeysRepository>();

        var keys = repository.GetAll().GetAwaiter().GetResult();

        var elements = new List<XElement>();
        foreach (var key in keys)
        {
            _logger.LogDebug("Reading XML from key '{FriendlyName}'", key.FriendlyName);
            if (!string.IsNullOrEmpty(key.Xml))
            {
                elements.Add(XElement.Parse(key.Xml));
            }
        }

        return elements.AsReadOnly();
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        using var scope = services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<DataProtectionKeysRepository>();

        _logger.LogDebug("Storing key '{FriendlyName}' in database", friendlyName);

        var newKey = new DataProtectionKey
        {
            FriendlyName = friendlyName,
            Xml = element.ToString(SaveOptions.DisableFormatting)
        };

        repository.Add(newKey).GetAwaiter().GetResult();
    }
}