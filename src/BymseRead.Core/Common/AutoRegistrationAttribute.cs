using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Core.Common;

[AttributeUsage(AttributeTargets.Class)]
public class AutoRegistrationAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;
}