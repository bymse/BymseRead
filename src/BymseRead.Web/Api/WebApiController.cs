using BymseRead.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Web.Api;

[Route("web-api/[controller]")]
[Controller]
public abstract class WebApiController : ControllerBase
{
    protected UserId CurrentUserId { get; } = null!;
}