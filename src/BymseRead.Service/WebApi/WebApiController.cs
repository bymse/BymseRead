using BymseRead.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.WebApi;

[Route("web-api/[controller]")]
[Controller]
public abstract class WebApiController : ControllerBase
{
    protected UserId CurrentUserId { get; } = null!;
}