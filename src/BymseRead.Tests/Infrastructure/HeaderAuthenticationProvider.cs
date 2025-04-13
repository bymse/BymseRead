using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace BymseRead.Tests.Infrastructure;

public class HeaderAuthenticationProvider(Guid userId) : IAuthenticationProvider
{
    public Task AuthenticateRequestAsync(
        RequestInformation request,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = new()
    )
    {
        request.Headers.Add(ServiceClientProvider.UserIdHeaderName, userId.ToString());
        return Task.CompletedTask;
    }
}