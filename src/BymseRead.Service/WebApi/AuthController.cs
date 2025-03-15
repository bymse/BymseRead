using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.WebApi;

[Route("web-api/[controller]")]
public class AuthController : Controller
{
    [HttpGet("login")]
    public ChallengeResult Login(string? returnUrl = null)
    {
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = Url.IsLocalUrl(returnUrl) ? returnUrl : "/",
        });
    }

    [HttpGet("logout")]
    public SignOutResult Logout()
    {
        return SignOut();
    }
}