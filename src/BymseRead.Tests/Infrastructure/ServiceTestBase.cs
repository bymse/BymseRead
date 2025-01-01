using BymseRead.Service.Client;
using BymseRead.Tests.Actions;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace BymseRead.Tests.Infrastructure;

public abstract class ServiceTestBase
{
    private readonly ServiceWebApplicationFactory _factory = new();

    protected ServiceActions Actions { get; private set; }
    
    protected HttpClient HttpClient { get; private set; }

    protected BymseReadClient GetServiceClient(Guid? userId = null)
    {
        var authProvider = new UserIdAuthProvider(userId ?? Actions.Users.CreateUser()
            .Result);

        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: _factory.CreateClient());
        return new BymseReadClient(adapter);
    }

    [OneTimeSetUp]
    public void SetUp()
    {
        Actions = new ServiceActions(new UsersActions());
        HttpClient = new HttpClient();
    }
}