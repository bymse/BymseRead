using System.Net.Http.Headers;
using System.Text;
using BymseRead.Core.Common;
using BymseRead.Service.Client.Models;
using BymseRead.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Tests.Actions;

[AutoRegistration(Lifetime = ServiceLifetime.Singleton)]
public class FilesActions(ServiceClientProvider provider)
{
    private static readonly HttpClient HttpClient = new();

    public static readonly string FileOtelPdfPath = Path.Combine("TestData", "file-otel.pdf");
    public static readonly string FileModernSePdfPath = Path.Combine("TestData", "file-modern-se.pdf");

    public static readonly string OtelPdfCoverPath = Path.Combine("TestData", "file-otel-cover.png");
    public static readonly string FileModernSePdfCoverPath = Path.Combine("TestData", "file-modern-se.png");

    public async Task<PreparedFileUploadResult> UploadFileFromPath(Guid userId, string path)
    {
        var data = await File.ReadAllBytesAsync(path);
        var fileName = Path.GetFileName(path);
        return await UploadFile(userId, fileName, data);
    }

    public async Task<PreparedFileUploadResult> UploadFile(Guid userId, string fileName, byte[] data)
    {
        var client = provider.Get(userId);

        using var content = new ByteArrayContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Headers.Add("x-amz-meta-originalFileName", fileName);

        var result = await client.WebApi.Files.PrepareUpload.PutAsync(new PrepareFileUploadRequest
        {
            FileName = fileName,
            FileSize = data.Length,
        });

        await UploadToUrl(result!.UploadUrl, result.EncodedFileName, data);

        return result;
    }

    public async Task UploadToUrl(string? uploadUrl, string? encodedFileName, byte[] data)
    {
        using var content = new ByteArrayContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Headers.Add("x-amz-meta-originalFileName", encodedFileName);

        var response = await HttpClient.PutAsync(new Uri(uploadUrl!), content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<PreparedFileUploadResult> UploadFile(
        Guid userId,
        string fileName,
        string fileContent = "hello world"
    ) =>
        await UploadFile(userId, fileName, Encoding.UTF8.GetBytes(fileContent));
}