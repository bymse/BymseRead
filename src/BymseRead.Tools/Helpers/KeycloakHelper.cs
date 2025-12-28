using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace BymseRead.Tools.Helpers;

public class KeycloakHelper(string url, string adminUsername, string adminPassword) : IDisposable
{
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri(url) };

    public async Task<string> InitializeKeycloakAsync(
        string realmName,
        string clientId,
        string rootUrl,
        string[] redirectUris,
        string username,
        string password)
    {
        var token = await GetAdminTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await CreateRealmAsync(realmName);
        var clientSecret = await CreateClientAsync(realmName, clientId, rootUrl, redirectUris);
        await CreateUserAsync(realmName, username, password);

        return clientSecret;
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var response = await _httpClient.PostAsync("realms/master/protocol/openid-connect/token",
            new FormUrlEncodedContent([
                new KeyValuePair<string, string>("client_id", "admin-cli"),
                new KeyValuePair<string, string>("username", adminUsername),
                new KeyValuePair<string, string>("password", adminPassword),
                new KeyValuePair<string, string>("grant_type", "password")
            ]));

        response.EnsureSuccessStatusCode();
        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

        return tokenResponse?.AccessToken ?? throw new InvalidOperationException("Failed to retrieve access token");
    }

    private async Task CreateRealmAsync(string realmName)
    {
        var realm = new
        {
            realm = realmName,
            enabled = true
        };

        var response = await _httpClient.PostAsJsonAsync("admin/realms", realm);
        if (response.StatusCode != HttpStatusCode.Conflict)
        {
            response.EnsureSuccessStatusCode();
        }
    }

    private async Task<string> CreateClientAsync(string realmName, string clientIdName, string rootUrl,
        string[] redirectUris)
    {
        var client = new
        {
            clientId = clientIdName,
            rootUrl,
            enabled = true,
            publicClient = false,
            redirectUris,
            webOrigins = new[]
            {
                "*"
            },
        };

        var response = await _httpClient.PostAsJsonAsync($"admin/realms/{realmName}/clients", client);
        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            var clientsResponse =
                await _httpClient.GetAsync($"admin/realms/{realmName}/clients?clientId={clientIdName}");
            var clients = await clientsResponse.Content.ReadFromJsonAsync<List<ClientRepresentation>>();
            return clients?[0].Secret ?? throw new InvalidOperationException("Failed to retrieve client secret");
        }

        var clientUrl = response.Headers.Location;
        response.EnsureSuccessStatusCode();
        var clientSecretResponse = await _httpClient.GetAsync(clientUrl);

        clientSecretResponse.EnsureSuccessStatusCode();
        var clientSecret = await clientSecretResponse.Content.ReadFromJsonAsync<ClientRepresentation>();

        return clientSecret?.Secret ?? throw new InvalidOperationException("Failed to retrieve client secret");
    }

    private async Task CreateUserAsync(string realmName, string username, string password)
    {
        var user = new
        {
            username,
            email = $"{username}@example.com",
            emailVerified = true,
            firstName = username,
            lastName = username,
            enabled = true,
            credentials = new[]
            {
                new
                {
                    type = "password",
                    value = password,
                    temporary = false
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync($"admin/realms/{realmName}/users", user);
        if (response.StatusCode != HttpStatusCode.Conflict)
        {
            response.EnsureSuccessStatusCode();
        }
    }

    private class TokenResponse
    {
        [JsonPropertyName("access_token")] public string AccessToken { get; set; }
    }

    private class ClientRepresentation
    {
        [JsonPropertyName("secret")] public string Secret { get; set; }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}