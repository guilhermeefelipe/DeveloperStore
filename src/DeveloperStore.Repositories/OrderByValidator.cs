using System.Reflection;

namespace DeveloperStore.Repositories;

public class OrderByValidator
{
    public static bool TryParseOrderBy(string orderBy, Type entityType, out string property, out bool descending)
    {
        property = null;
        descending = false;

        if (string.IsNullOrWhiteSpace(orderBy))
            return false;

        // Split the orderBy string into parts
        var parts = orderBy.Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var trimmedPart = part.Trim();
            var direction = trimmedPart.EndsWith(" desc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";
            var field = direction == "desc" ? trimmedPart.Substring(0, trimmedPart.Length - 5).Trim() : trimmedPart;

            // Check for the property in the entity type and its base types
            var propInfo = entityType.GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propInfo == null)
            {
                // If not found, check the base class
                var baseType = entityType.BaseType;
                while (baseType != null)
                {
                    propInfo = baseType.GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propInfo != null)
                        break;

                    baseType = baseType.BaseType;
                }
            }

            if (propInfo == null)
                return false; // If any property is invalid, return false

            // Set the property and direction
            property = field;
            descending = direction == "desc";
            return true; // Return true if the property is valid
        }

        return false; // If no valid properties were found
    }
}