using System.Net;
using System.Net.Http.Headers;
using BymseRead.Service.Client.Models;
using BymseRead.Tests.Infrastructure;
using FluentAssertions;

namespace BymseRead.Tests.WebApiTests;

public class FilesTests : ServiceTestBase
{
    private const string FileName = "book.pdf";
    private const string FileContent = "hello world";

    [Test]
    public async Task Should_UploadFile_OnPrepareUpload()
    {
        var user = Actions.Users.CreateUser();

        await Actions
            .Files.Invoking(async e => await e.UploadFile(user, FileName))
            .Should()
            .NotThrowAsync();
    }

    [TestCase(".txt")]
    [TestCase(".exe")]
    [TestCase("")]
    public async Task Should_Throw_OnUnknownExtension(string extension)
    {
        var fileName = $"book{extension}";

        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var problem = await client
            .WebApi.Files.PrepareUpload.Invoking(e =>
                e.PutAsync(new PrepareFileUploadRequest { FileName = fileName, FileSize = 0 }))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("File extension");
    }

    [TestCase(".pdf", 1024 * 1024 * 300 + 1)]
    [TestCase(".png", 5 * 1024 * 1024 + 1)]
    [TestCase(".jpeg", 5 * 1024 * 1024 + 1)]
    public async Task Should_Throw_OnTooLargeFile(string extension, int size)
    {
        var fileName = $"book{extension}";

        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var problem = await client
            .WebApi.Files.PrepareUpload.Invoking(e =>
                e.PutAsync(new PrepareFileUploadRequest { FileName = fileName, FileSize = size }))
            .Should()
            .ThrowAsync<ProblemDetails>();

        problem
            .Which.Detail.Should()
            .Contain("File size");
    }

    [Test]
    public async Task Should_Fail_OnUploadWithWrongOriginalName()
    {
        using var content = new StringContent(FileContent);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Headers.Add("x-amz-meta-originalFileName", "random");

        var prepareRequest = new PrepareFileUploadRequest { FileName = FileName, FileSize = FileContent.Length, };
        await AssertFileUploadForbidden(prepareRequest, content);
    }

    [Test]
    public async Task Should_Fail_OnUploadWithWrongContentType()
    {
        using var content = new StringContent(FileContent);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        content.Headers.Add("x-amz-meta-originalFileName", FileName);

        var prepareRequest = new PrepareFileUploadRequest { FileName = FileName, FileSize = FileContent.Length, };
        await AssertFileUploadForbidden(prepareRequest, content);
    }

    [Test]
    public async Task Should_Fail_OnUploadWithWrongContentLength()
    {
        using var content = new StringContent(FileContent);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        content.Headers.Add("x-amz-meta-originalFileName", FileName);

        var prepareRequest = new PrepareFileUploadRequest { FileName = FileName, FileSize = FileContent.Length + 1, };
        await AssertFileUploadForbidden(prepareRequest, content);
    }

    private async Task AssertFileUploadForbidden(PrepareFileUploadRequest request, StringContent content)
    {
        var user = Actions.Users.CreateUser();
        var client = GetServiceClient(user);

        var result = await client.WebApi.Files.PrepareUpload.PutAsync(request);

        var response = await HttpClient.PutAsync(new Uri(result!.UploadUrl!), content);
        response
            .StatusCode.Should()
            .Be(HttpStatusCode.Forbidden);
    }
}