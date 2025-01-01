using System.Data;
using BymseRead.Core.Entities;
using Dapper;

namespace BymseRead.Infrastructure.Database.Dapper;

public class ValueObjectTypeHandler(Type type) : SqlMapper.ITypeHandler
{
    public void SetValue(IDbDataParameter parameter, object value)
    {
        parameter.Value = ((IEntityId) value).Value;
    }

    public object? Parse(Type destinationType, object value)
    {
        return value switch
        {
            Guid guid => Activator.CreateInstance(type, guid)!,
            string str => Activator.CreateInstance(type, Guid.Parse(str))!,
            _ => throw new NotSupportedException(),
        };
    }
}