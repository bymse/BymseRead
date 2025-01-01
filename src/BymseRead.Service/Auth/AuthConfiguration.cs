using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BymseRead.Service.Auth;

public static class AuthConfiguration
{
    public static IServiceCollection AddAuthN(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(e =>
            {
                var settings = configuration
                    .GetSection(AuthNSettings.Path)
                    .Get<AuthNSettings>() ?? throw new InvalidOperationException("Missing AuthN settings");

                e.Authority = settings.Authority;
                e.Audience = settings.Audience;
                e.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = settings.Audience != null,
                    ValidAudience = settings.Audience,
                    ValidIssuer = settings.Issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = settings.SymmetricKey != null
                        ? new SymmetricSecurityKey(Encoding.Default.GetBytes(settings.SymmetricKey))
                        : null,
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
    public string? Audience { get; init; }
    public string? Issuer { get; init; }

    public string? SymmetricKey { get; init; }
}