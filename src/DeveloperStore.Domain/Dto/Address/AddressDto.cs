using DeveloperStore.Domain.Dto.Geolocation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DeveloperStore.Domain.Dto.Address;

public class AddressDto
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public GeolocationDto Geolocation { get; set; } = default!;
}