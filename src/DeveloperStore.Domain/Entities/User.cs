using DeveloperStore.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeveloperStore.Domain.Entities;

public class User : SimpleEntityBase
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int NameId { get; set; }

    [ForeignKey(nameof(NameId))]
    public Name Name { get; set; } = default!;
    public int AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public Address Address { get; set; } = default!;

    public string Phone { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Role Role { get; set; }
}
