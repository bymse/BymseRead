using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BymseRead.Service.WebApi;

[Route("web-api/[controller]")]
public class AuthController(IConfiguration configuration) : Controller
{

    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        var allowedHosts = configuration.GetSection("ReturnUrlAllowList").Get<string[]>() ?? [];
        var isValidReturnUrl = !string.IsNullOrEmpty(returnUrl) && allowedHosts.Any(returnUrl.StartsWith);

        return Challenge(new AuthenticationProperties
        {
            RedirectUri = isValidReturnUrl ? returnUrl : "/",
        });
    }

    [HttpGet("logout")]
    public SignOutResult Logout()
    {
        return SignOut();
    }
}