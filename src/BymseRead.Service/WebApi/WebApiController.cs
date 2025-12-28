using System.Security.Claims;
using BymseRead.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.WebApi;

[Route("web-api/[controller]")]
[Authorize]
[ApiController]
public abstract class WebApiController : ControllerBase
{
    public const string DocumentName = "WebApi";

    protected UserId CurrentUserId => new(Guid.Parse(Request.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!));
}