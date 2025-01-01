using BymseRead.Service.Client;
using BymseRead.Tests.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Tests.Infrastructure;

public abstract class ServiceTestBase
{
    private readonly ServiceWebApplicationFactory _factory = new();

    protected ServiceActions Actions { get; private set; }

    private ServiceClientProvider _serviceClientProvider;

    protected HttpClient HttpClient { get; private set; }

    protected BymseReadClient GetServiceClient(Guid userId) => _serviceClientProvider.Get(userId);

    [OneTimeSetUp]
    public void SetUp()
    {
        HttpClient = new HttpClient();

        _serviceClientProvider = _factory.Services.GetRequiredService<ServiceClientProvider>();
        Actions = _factory.Services.GetRequiredService<ServiceActions>();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _factory.Dispose();
        HttpClient.Dispose();
    }
}