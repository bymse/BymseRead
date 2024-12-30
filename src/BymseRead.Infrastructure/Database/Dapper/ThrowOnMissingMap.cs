using System.Reflection;
using Dapper;

namespace BymseRead.Infrastructure.Database.Dapper;

public class ThrowOnMissingMap(Type type) : SqlMapper.ITypeMap
{
    private readonly DefaultTypeMap _defaultTypeMap = new(type);
    
    public ConstructorInfo? FindConstructor(string[] names, Type[] types)
    {
        return _defaultTypeMap.FindConstructor(names, types);
    }

    public ConstructorInfo? FindExplicitConstructor()
    {
        return _defaultTypeMap.FindExplicitConstructor();
    }

    public SqlMapper.IMemberMap? GetConstructorParameter(ConstructorInfo constructor, string columnName)
    {
        return _defaultTypeMap.GetConstructorParameter(constructor, columnName);
    }

    public SqlMapper.IMemberMap? GetMember(string columnName)
    {
        var member = _defaultTypeMap.GetMember(columnName);
        if (member == null)
        {
            throw new InvalidOperationException($"Column '{columnName}' not found in type '{type.Name}'");
        }
        
        return member;
    }
}