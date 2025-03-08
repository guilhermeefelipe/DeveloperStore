using DeveloperStore.Domain.Dto.Base;

namespace DeveloperStore.Domain.Dto.Geolocation;

public class GeolocationCompleteDto : SimpleDtoBase
{
    public string Lat { get; set; } = string.Empty;
    public string Long { get; set; } = string.Empty;
}
