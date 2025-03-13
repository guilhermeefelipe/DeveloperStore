using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Services.Services;

public class EnumValidator
{
    public static void ValidateRole(string role)
    {
        var normalizedRole = role?.ToLowerInvariant();
        var validRoles = Enum.GetNames(typeof(Role)).Select(r => r.ToLowerInvariant()).ToArray();

        if (!validRoles.Contains(normalizedRole))
        {
            throw new CustomException("ValidationError", "Invalid role provided", $"Valid roles are: {string.Join(", ", Enum.GetNames(typeof(Role)))}");
        }
    }

    public static void ValidateStatus(string status)
    {
        var normalizedStatus = status?.ToLowerInvariant();
        var validStatuses = Enum.GetNames(typeof(Status)).Select(s => s.ToLowerInvariant()).ToArray();

        if (!validStatuses.Contains(normalizedStatus))
        {
            throw new CustomException("ValidationError", "Invalid status provided", $"Valid statuses are: {string.Join(", ", Enum.GetNames(typeof(Status)))}");
        }
    }
}