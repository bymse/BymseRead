using BymseRead.Core.Common;
using BymseRead.Service.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace BymseRead.Tests.Infrastructure;

[AutoRegistration(Lifetime = ServiceLifetime.Transient)]
public class ServiceClientProvider(HttpClient httpClient)
{
    public const string UserIdHeaderName = "X-Test-User-Id";

    public BymseReadClient Get(Guid userId)
    {
        var authProvider = new HeaderAuthenticationProvider(userId);
        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);

        return new BymseReadClient(adapter);
    }
}