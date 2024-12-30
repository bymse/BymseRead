using System.Data;
using BymseRead.Core.Entities;
using Dapper;

namespace BymseRead.Infrastructure.Database.Dapper;

public class EntityIdTypeHandler(Type type) : SqlMapper.ITypeHandler
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
            _ => throw new NotSupportedException(),
        };
    }
}