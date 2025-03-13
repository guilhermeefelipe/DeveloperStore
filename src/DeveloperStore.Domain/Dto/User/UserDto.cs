using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Domain.Dto.Name;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Dto.Users;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public NameDto Name { get; set; } = default!;
    public AddressDto Address { get; set; } = default!;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
