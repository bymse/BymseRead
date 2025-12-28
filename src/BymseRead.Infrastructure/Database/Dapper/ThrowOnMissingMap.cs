using System.Reflection;
using Dapper;

namespace BymseRead.Infrastructure.Database.Dapper;

public class ThrowOnMissingMap(Type type) : SqlMapper.ITypeMap
{
    private readonly DefaultTypeMap defaultTypeMap = new(type);

    public ConstructorInfo? FindConstructor(string[] names, Type[] types)
    {
        return defaultTypeMap.FindConstructor(names, types);
    }

    public ConstructorInfo? FindExplicitConstructor()
    {
        return defaultTypeMap.FindExplicitConstructor();
    }

    public SqlMapper.IMemberMap? GetConstructorParameter(ConstructorInfo constructor, string columnName)
    {
        return defaultTypeMap.GetConstructorParameter(constructor, columnName);
    }

    public SqlMapper.IMemberMap? GetMember(string columnName)
    {
        var member = defaultTypeMap.GetMember(columnName);
        if (member == null)
        {
            throw new InvalidOperationException($"Column '{columnName}' not found in type '{type.Name}'");
        }

        return member;
    }
}