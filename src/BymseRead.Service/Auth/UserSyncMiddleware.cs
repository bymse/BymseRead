using System.Security.Claims;
using BymseRead.Core.Application.SyncUser;

namespace BymseRead.Service.Auth;

public class UserSyncMiddleware(RequestDelegate next)
{
    public const string UserIdKey = "BymseReadUserId";
    
    public async Task InvokeAsync(HttpContext context)
    {
        var externalUserId = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (externalUserId != null)
        {
            var handler = context.RequestServices.GetRequiredService<SyncUserHandler>();
            context.Items[UserIdKey] = await handler.Handle(externalUserId.Issuer, externalUserId.Value);
        }

        await next(context);
    }
}