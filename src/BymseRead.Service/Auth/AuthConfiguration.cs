using System.Security.Claims;
using BymseRead.Core.Application.SyncUser;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace BymseRead.Service.Auth;

public static class AuthConfiguration
{
    private const string ProxyScheme = "Proxy";

    public static IServiceCollection AddAuthN(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        services
            .AddAuthorization()
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = ProxyScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, ProxyAuthenticationHandler>(ProxyScheme, null)
            .AddCookie(options => { options.SlidingExpiration = true; })
            .AddOpenIdConnect(e =>
            {
                var settings = configuration
                    .GetSection(AuthNSettings.Path)
                    .Get<AuthNSettings>() ?? throw new InvalidOperationException("Missing AuthN settings");

                e.Authority = settings.Authority;
                e.ClientId = settings.ClientId;
                e.ClientSecret = settings.ClientSecret;
                e.ResponseType = "code";
                e.CallbackPath = "/web-api/auth/signin-oidc";
                e.SignedOutCallbackPath = "/web-api/auth/signout-callback-oidc";
                e.DisableTelemetry = true;
                e.Scope.Add("openid");
                e.RequireHttpsMetadata = !environment.IsDevelopment();
                e.MetadataAddress = settings.MetadataAddress;
                e.MapInboundClaims = false;

                e.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };

                e.Events.OnTokenValidated = async context =>
                {
                    var syncUserHandler = context.HttpContext.RequestServices.GetRequiredService<SyncUserHandler>();
                    var idpUserId = context.Principal?.FindFirst("sub") ?? throw new InvalidOperationException("Missing sub claim");

                    var userId = await syncUserHandler.Handle(idpUserId.Issuer, idpUserId.Value);

                    var identity = context.Principal?.Identity as ClaimsIdentity
                                   ?? throw new InvalidOperationException("Invalid principal identity");

                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()));
                };
            });

        return services;
    }

    public static void UseAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}

public class AuthNSettings
{
    public const string Path = "AuthN";

    public string? Authority { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? MetadataAddress { get; init; }
}