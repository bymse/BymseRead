using BymseRead.Service.Auth;
using BymseRead.Service.Client;
using BymseRead.Tests.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace BymseRead.Tests.Infrastructure;

public abstract class ServiceTestBase
{
    private readonly ServiceWebApplicationFactory _factory = new();
    private AuthNSettings _authNSettings;

    protected ServiceActions Actions { get; private set; }

    protected HttpClient HttpClient { get; private set; }

    protected BymseReadClient GetServiceClient(Guid? externalUserId = null)
    {
        var userId = externalUserId ?? Actions.Users.CreateUser();

        var authProvider = new BaseBearerTokenAuthenticationProvider(
            new GeneratedAccessTokenProvider(userId, _authNSettings));

        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: _factory.CreateClient());
        return new BymseReadClient(adapter);
    }

    [OneTimeSetUp]
    public void SetUp()
    {
        Actions = new ServiceActions(new UsersActions());
        HttpClient = new HttpClient();

        var configuration = _factory.Services.GetRequiredService<IConfiguration>();
        _authNSettings = configuration
            .GetRequiredSection(AuthNSettings.Path)
            .Get<AuthNSettings>()!;
    }
    
    [OneTimeTearDown]
    public void TearDown()
    {
        _factory.Dispose();
        HttpClient.Dispose();
    }
}