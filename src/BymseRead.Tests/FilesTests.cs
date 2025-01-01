using BymseRead.Tests.Infrastructure;
using FluentAssertions;

namespace BymseRead.Tests;

public class FilesTests : ServiceTestBase
{
    [Test]
    public async Task Should_UploadFile_OnPrepareUpload()
    {
        const string fileName = "book.pdf";

        var user = Actions.Users.CreateUser();

        await Actions
            .Files.Invoking(async e => await e.UploadFile(user, fileName))
            .Should()
            .NotThrowAsync();
    }
}