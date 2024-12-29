using System.Reflection;

namespace BymseRead.Legacy.DataLayer.Helpers;

[AttributeUsage(AttributeTargets.Field)]
public class AttachedValueAttribute<T> : Attribute
{
    public T Value { get; }

    public AttachedValueAttribute(T value)
    {
        Value = value;
    }
}

public static class AttachedValueHelper<T, TV> where T : struct, Enum
{
    private static readonly Lazy<IReadOnlyDictionary<T, TV>> Values = new(
        () => Enum.GetValues<T>().ToDictionary(e => e, GetWithReflection)
    );

    public static TV GetValue(T obj) => Values.Value[obj];

    private static TV GetWithReflection(T obj)
    {
        var field = obj.GetType().GetField(obj.ToString())
                    ?? throw new NullReferenceException("Field not found");
        var attribute = field.GetCustomAttribute<AttachedValueAttribute<TV>>() ??
                        throw new NullReferenceException("Attribute not found");

        return attribute.Value;
    }
}