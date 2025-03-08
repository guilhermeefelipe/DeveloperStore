using DeveloperStore.Domain.Dto.Base;
using DeveloperStore.Domain.Dto.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Dto.Address;

public class AddressCompleteDto : SimpleDtoBase
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public GeolocationCompleteDto Geolocation { get; set; } = default!;
}
