using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace BymseRead.Tests.Infrastructure;

public class UserIdAuthProvider(Guid id) : IAuthenticationProvider
{
    public Task AuthenticateRequestAsync(
        RequestInformation request,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = new()
    )
    {
        request.Headers.Add("X-User-Id", id.ToString());
        return Task.CompletedTask;
    }
}