using System.Net;
using BymseRead.Service.Client;
using BymseRead.Tests.Actions;
using FluentAssertions;
using FluentAssertions.Extensions;
using ImageMagick;
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

    protected async Task AssertContent(string? url, string expectedContent)
    {
        var fileResponse = await HttpClient.GetAsync(url!);
        fileResponse.EnsureSuccessStatusCode();

        var fileContent = await fileResponse.Content.ReadAsStringAsync();
        fileContent
            .Should()
            .Be(expectedContent);
    }
    
    protected async Task AssertCover(Guid userId, Guid bookId, string expectedContentPath)
    {
        await Task.Delay(10.Seconds());
        var client = GetServiceClient(userId);
        
        var book = await client
            .WebApi.Books[bookId]
            .GetAsync();
        
        var fileResponse = await HttpClient.GetAsync(book!.CoverUrl);
        fileResponse.EnsureSuccessStatusCode();

        using var expectedImage = new MagickImage(await File.ReadAllBytesAsync(expectedContentPath));
        using var actualImage = new MagickImage(await fileResponse.Content.ReadAsByteArrayAsync());
        
        var diff = expectedImage.Compare(actualImage, ErrorMetric.Absolute);
        diff
            .Should()
            .BeLessThan(1);
    }
    
    protected async Task AssertNotFound(string? url)
    {
        if (url == null)
        {
            return;
        }
        
        var fileResponse = await HttpClient.GetAsync(url!);
        fileResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }
}