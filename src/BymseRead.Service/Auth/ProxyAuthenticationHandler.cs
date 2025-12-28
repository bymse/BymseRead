using System.Text.Encodings.Web;
using BymseRead.Service.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BymseRead.Service.Auth;

public class ProxyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private const string RedirectUrl = "/web-api/auth/login";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return Task.FromResult(AuthenticateResult.NoResult());
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        var endpoint = Context.GetEndpoint();
        var isApiController = endpoint?.Metadata?.GetMetadata<ApiControllerAttribute>() != null;

        if (isApiController)
        {
            Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            Context.Response.ContentType = "application/problem+json";

            var problemDetails = new RedirectProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Authentication required to access this resource",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                RedirectUrl = RedirectUrl,
            };

            var problemDetailsService = Context.RequestServices.GetRequiredService<IProblemDetailsService>();
            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = Context,
                ProblemDetails = problemDetails
            });
        }
        else
        {
            await Context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, properties);
        }
    }
}