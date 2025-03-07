using DeveloperStore.Domain.Entities.Base;

namespace DeveloperStore.Domain.Entities;

public class Geolocation : SimpleEntityBase
{
    public string Lat { get; set; } = string.Empty;
    public string Long { get; set; } = string.Empty;
}