using BymseRead.Core.Entities;
using BymseRead.Service.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.WebApi;

[Route("web-api/[controller]")]
[Authorize]
[ApiController]
public abstract class WebApiController : ControllerBase
{
    public const string DocumentName = "WebApi";

    protected UserId CurrentUserId => (UserId) Request.HttpContext.Items[UserSyncMiddleware.UserIdKey]!;
}