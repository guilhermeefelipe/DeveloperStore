using System.Text.Json.Serialization;

namespace DeveloperStore.Domain.Dto.Name;

public class NameDto
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
}