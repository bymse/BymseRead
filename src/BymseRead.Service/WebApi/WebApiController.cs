using BymseRead.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.WebApi;

[Route("web-api/[controller]")]
[Controller]
public abstract class WebApiController : ControllerBase
{
    public const string DocumentName = "WebApi";
    
    protected UserId CurrentUserId { get; } = new UserId(Guid.NewGuid());
}