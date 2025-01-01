using BymseRead.Core.Common;
using BymseRead.Service.Auth;
using BymseRead.Service.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace BymseRead.Tests.Infrastructure;

[AutoRegistration(Lifetime = ServiceLifetime.Singleton)]
public class ServiceClientBuilder(HttpClient httpClient, IConfiguration configuration)
{
    private readonly AuthNSettings _authNSettings = configuration
        .GetRequiredSection(AuthNSettings.Path)
        .Get<AuthNSettings>()!;
    
    public BymseReadClient Build(Guid userId)
    {
        var authProvider = new BaseBearerTokenAuthenticationProvider(
            new GeneratedAccessTokenProvider(userId, _authNSettings));

        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);
        return new BymseReadClient(adapter);
    }
}