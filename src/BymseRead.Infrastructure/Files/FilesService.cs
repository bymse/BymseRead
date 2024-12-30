using BymseRead.Core.Common;
using BymseRead.Core.Services;
using File = BymseRead.Core.Entities.File;

namespace BymseRead.Infrastructure.Files;

[AutoRegistration]
public class FilesService : IFilesService
{
    public Uri GetUrl(File file)
    {
        return new Uri("https://example.com");
    }
}