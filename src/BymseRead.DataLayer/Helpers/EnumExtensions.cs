namespace BymseRead.DataLayer.Helpers;

public static class EnumExtensions
{
    public static string GetAttachedString<T>(this T obj) where T : struct, Enum
        => GetAttachedValue<T, string>(obj);
    
    public static TV GetAttachedValue<T, TV>(this T obj) where T : struct, Enum
        => AttachedValueHelper<T, TV>.GetValue(obj);
}