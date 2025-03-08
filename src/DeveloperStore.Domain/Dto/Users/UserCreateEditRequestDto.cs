using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Domain.Dto.Name;
using DeveloperStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Dto.Users;

public class UserCreateEditRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public NameDto Name { get; set; } = default!;
    public AddressDto Address { get; set; } = default!;
    public string Phone { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Role Role { get; set; }
}
