using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Domain.Dto.Base;
using DeveloperStore.Domain.Dto.Name;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Dto.Users;

public class UserCompleteDto : SimpleDtoBase
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public NameCompleteDto Name { get; set; } = default!;
    public AddressCompleteDto Address { get; set; } = default!;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
