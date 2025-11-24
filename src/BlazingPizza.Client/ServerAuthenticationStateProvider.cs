using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazingPizza.Client;

public class ServerAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public ServerAuthenticationStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userInfo = await _httpClient.GetFromJsonAsync<UserInfo>("api/user");

            if (userInfo != null && userInfo.IsAuthenticated)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userInfo.UserName ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, userInfo.UserId ?? "")
                };

                var identity = new ClaimsIdentity(claims, "serverauth");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        }
        catch
        {
            // User is not authenticated
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}

public class UserInfo
{
    public bool IsAuthenticated { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set; }
}
