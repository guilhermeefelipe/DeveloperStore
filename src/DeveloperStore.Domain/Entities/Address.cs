using DeveloperStore.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Entities;

public class Address : SimpleEntityBase
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public int GeolocationId { get; set; }

    [ForeignKey(nameof(GeolocationId))]
    public Geolocation Geolocation { get; set; } = default!;

}