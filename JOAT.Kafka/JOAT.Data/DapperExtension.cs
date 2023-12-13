using Dapper;

namespace JOAT.Data;

public static class DapperExtension
{
    public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>(
        this IEnumerable<T> enumerable,
        string typeName,
        IEnumerable<string> orderedColumnNames = null)
    {
        return enumerable.AsDataTable(orderedColumnNames).AsTableValuedParameter(typeName);
    }
}