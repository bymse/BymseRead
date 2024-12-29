using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Core.Common;

public static class AutoRegistrationHelper
{
    public static IServiceCollection AddAutoRegistrations(this IServiceCollection services, params Assembly[] assembly)
    {
        foreach (var asm in assembly)
        {
            foreach (var type in asm.GetTypes().Where(e => e is { IsClass: true, IsAbstract: false }))
            {
                if (type.GetCustomAttribute<AutoRegistrationAttribute>() is null)
                {
                    continue;
                }
                
                services.AddScoped(type);

                foreach (var @interface in type.GetInterfaces())
                {
                    services.AddScoped(@interface, type);
                }
            }
        }

        return services;
    }
}