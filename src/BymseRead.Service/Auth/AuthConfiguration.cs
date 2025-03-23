using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;

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
            .AddCookie(options =>
            {
                options.SlidingExpiration = true;
            })
            .AddOpenIdConnect(e =>
            {
                var settings = configuration
                    .GetSection(AuthNSettings.Path)
                    .Get<AuthNSettings>() ?? throw new InvalidOperationException("Missing AuthN settings");

                e.Authority = settings.Authority;
                e.ClientId = settings.ClientId;
                e.ClientSecret = settings.ClientSecret;
                e.ResponseType = settings.ResponseType ?? "code";
                e.CallbackPath = settings.CallbackPath;
                e.SignedOutCallbackPath = settings.SignedOutCallbackPath;
                e.SaveTokens = true;
                e.DisableTelemetry = true;
                e.Scope.Add("openid");
                e.Scope.Add("profile");
                e.RequireHttpsMetadata = !environment.IsDevelopment();
                e.MetadataAddress = settings.MetadataAddress;
                e.MapInboundClaims = false;

                e.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true, ValidateIssuerSigningKey = true,
                };
            });

        return services;
    }

    public static void UseAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseMiddleware<UserSyncMiddleware>();
        app.UseAuthorization();
    }
}

public class AuthNSettings
{
    public const string Path = "AuthN";

    public string? Authority { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? ResponseType { get; init; }
    public string? CallbackPath { get; init; }
    public string? SignedOutCallbackPath { get; init; }
    public string? MetadataAddress { get; init; }
}