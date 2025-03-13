using System.Reflection;

namespace DeveloperStore.Repositories;

public class OrderByValidator
{
    public static bool TryParseOrderBy(string orderByClause, Type entityType, out string property, out bool descending)
    {
        property = null;
        descending = false;

        if (string.IsNullOrWhiteSpace(orderByClause))
            return false;

        var trimmedClause = orderByClause.Trim();
        if (trimmedClause.EndsWith(" desc", StringComparison.OrdinalIgnoreCase))
        {
            descending = true;
            property = trimmedClause.Substring(0, trimmedClause.Length - 5).Trim(); 
        }
        else if (trimmedClause.EndsWith(" asc", StringComparison.OrdinalIgnoreCase))
        {
            property = trimmedClause.Substring(0, trimmedClause.Length - 4).Trim();
        }
        else
        {
            property = trimmedClause;
        }

        var propInfo = entityType.GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (propInfo == null)
        {
            var baseType = entityType.BaseType;
            while (baseType != null)
            {
                propInfo = baseType.GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propInfo != null)
                    break;

                baseType = baseType.BaseType;
            }
        }

        return propInfo != null;
    }
}