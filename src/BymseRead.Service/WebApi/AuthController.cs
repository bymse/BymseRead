using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.WebApi;

[Route("web-api/[controller]")]
public class AuthController(IConfiguration configuration) : Controller
{
    [AllowAnonymous]
    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        var allowedHosts = configuration.GetSection("ReturnUrlAllowList").Get<string[]>() ?? [];
        var isValidReturnUrl = !string.IsNullOrEmpty(returnUrl) && allowedHosts.Any(returnUrl.StartsWith);

        return Challenge(new AuthenticationProperties
        {
            RedirectUri = isValidReturnUrl ? returnUrl : "/",
        }, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet("logout")]
    public SignOutResult Logout()
    {
        return SignOut();
    }
}