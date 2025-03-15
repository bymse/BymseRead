using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BymseRead.Service.Auth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Kiota.Abstractions.Authentication;

namespace BymseRead.Tests.Infrastructure
{
    public class GeneratedAccessTokenProvider(Guid userId, AuthNSettings settings) : IAccessTokenProvider
    {
        public Task<string> GetAuthorizationTokenAsync(
            Uri uri,
            Dictionary<string, object>? additionalAuthenticationContext = null,
            CancellationToken cancellationToken = new()
        )
        {
            // var claims = new[]
            // {
            //     new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            //     new Claim(JwtRegisteredClaimNames.Iss, settings.Issuer!),
            // };
            //
            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SymmetricKey!));
            // var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //
            // var token = new JwtSecurityToken(issuer: settings.Issuer,
            //     audience: settings.Audience,
            //     claims: claims,
            //     expires: DateTime.Now.AddMinutes(30),
            //     signingCredentials: credentials);
            //
            // return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
            return Task.FromResult(string.Empty);
        }

        public AllowedHostsValidator AllowedHostsValidator { get; } = new();
    }
}