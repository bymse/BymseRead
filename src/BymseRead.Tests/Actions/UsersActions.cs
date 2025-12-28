using BymseRead.Core.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Tests.Actions;

[AutoRegistration(Lifetime = ServiceLifetime.Singleton)]
public class UsersActions
{
    public Guid CreateUser()
    {
        return Guid.NewGuid();
    }
}