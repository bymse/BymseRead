using System.Security.Claims;
using System.Text.Encodings.Web;
using BymseRead.Core.Application.SyncUser;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BymseRead.Tests.Infrastructure;

public class TestHeaderAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    SyncUserHandler syncUserHandler
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ServiceClientProvider.UserIdHeaderName, out var userIdValues) ||
            userIdValues.Count == 0)
        {
            return AuthenticateResult.NoResult();
        }

        var userGuidRaw = userIdValues[0];
        if (string.IsNullOrEmpty(userGuidRaw) || !Guid.TryParse(userGuidRaw, out var userGuid))
        {
            return AuthenticateResult.Fail("Invalid user ID format");
        }

        var userId = await syncUserHandler.Handle("test-idp", userGuid.ToString());
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()),
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}