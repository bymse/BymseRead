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
                var attr = type.GetCustomAttribute<AutoRegistrationAttribute>();
                if (attr is null)
                {
                    continue;
                }

                services.Add(new ServiceDescriptor(type, type, attr.Lifetime));

                foreach (var @interface in type.GetInterfaces())
                {
                    services.Add(new ServiceDescriptor(@interface, type, attr.Lifetime));
                }
            }
        }

        return services;
    }
}