using System.Net.Http.Headers;
using BymseRead.Core.Common;
using BymseRead.Service.Client.Models;
using BymseRead.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Tests.Actions;

[AutoRegistration(Lifetime = ServiceLifetime.Singleton)]
public class FilesActions(ServiceClientProvider provider)
{
    private static readonly HttpClient HttpClient = new();

    public async Task<PreparedFileUploadResult> UploadFile(
        Guid userId,
        string fileName,
        string fileContent = "hello world"
    )
    {
        var client = provider.Get(userId);

        using var content = new StringContent(fileContent);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Headers.Add("x-amz-meta-originalFileName", fileName);

        var result = await client.WebApi.Files.PrepareUpload.PutAsync(new PrepareFileUploadRequest
        {
            FileName = fileName, FileSize = fileContent.Length,
        });

        var response = await HttpClient.PutAsync(new Uri(result!.UploadUrl!), content);
        response.EnsureSuccessStatusCode();

        return result;
    }
}