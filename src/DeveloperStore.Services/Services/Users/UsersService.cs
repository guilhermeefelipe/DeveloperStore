using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Domain.Dto.User;
using DeveloperStore.Domain.Dto.Users;
using DeveloperStore.Repositories.Addresses;
using DeveloperStore.Repositories.Geolocations;
using DeveloperStore.Repositories.Names;
using DeveloperStore.Repositories.Repositories.Carts;
using DeveloperStore.Repositories.Users;
using DeveloperStore.Services.Services;
using Newtonsoft.Json.Linq;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace DeveloperStore.Services.Users;

public interface IUsersService
{
    Task<UserDto?> CreateAsync(UserCreateEditRequestDto model);
    Task<UserDto?> UpdateAsync(int id, UserCreateEditRequestDto model);
    Task<bool> DeleteAsync(int id);
    Task<UserDto?> GetAsync(int id);    
    Task<IPagedList<UserDto>> GetPagedListAsync(int page, int pageSize, string order);
    Task<string> ValidateLogin(UserLoginDto model);

}

public class UsersService : IUsersService
{
    private readonly IUsersRepository usersRepository;
    private readonly INamesRepository namesRepository;
    private readonly IGeolocationsRepository geolocationsRepository;
    private readonly IAddressesRepository addressesRepository;

    public UsersService(IUsersRepository usersRepository,
                        INamesRepository namesRepository,
                        IGeolocationsRepository geolocationsRepository,
                        IAddressesRepository addressesRepository)
    {
        this.usersRepository = usersRepository;
        this.namesRepository = namesRepository;
        this.geolocationsRepository = geolocationsRepository;
        this.addressesRepository = addressesRepository;
    }

    public async Task<UserDto?> CreateAsync(UserCreateEditRequestDto model)
    {
        if (model == null)
            throw new CustomException("InvalidRequest", "Request is invalid", "Request is not in the expected standard");

        var geolocationId = await geolocationsRepository.CreateAsync(model.Address.Geolocation);

        var addressId = await addressesRepository.CreateAsync(new AddressCreateEditDto
        {  
            City = model.Address.City,
            GeolocationId = geolocationId,
            Number = model.Address.Number,
            Street = model.Address.Street,
            ZipCode = model.Address.ZipCode
        });

        var nameId = await namesRepository.CreateAsync(model.Name);

        var userId = await usersRepository.CreateAsync(new UserCreateEditDto
        {
            AddressId = addressId,
            NameId = nameId,
            Email = model.Email,
            Password = CryptoHelper.Encrypt(model.Password),
            Phone = model.Phone,
            Role = model.Role,
            Status = model.Status,
            Username = model.Username
        });

        return await GetAsync(userId);
    }
    public async Task<UserDto?> UpdateAsync(int id, UserCreateEditRequestDto model)
    {
        if (model == null)
            throw new CustomException("InvalidRequest", "Request is invalid", "Request is not in the expected standard");

        var user = await usersRepository.GetAsync<UserCompleteDto>(id);

        if (user == null)
            throw new CustomException("UserNotFound", "User not found", $"No user found with these id:{id}");

        await geolocationsRepository.UpdateAsync(user.Address.Geolocation.Id, model.Address.Geolocation);

        await addressesRepository.UpdateAsync(user.Address.Id, new AddressCreateEditDto
        {
            City = model.Address.City,
            GeolocationId = user.Address.Geolocation.Id,
            Number = model.Address.Number,
            Street = model.Address.Street,
            ZipCode = model.Address.ZipCode
        });

        await namesRepository.UpdateAsync(user.Name.Id, model.Name);

        await usersRepository.UpdateAsync(id, new UserCreateEditDto
        {
            AddressId = user.Address.Id,
            NameId = user.Name.Id,
            Email = model.Email,
            Password = CryptoHelper.Encrypt(model.Password),
            Phone = model.Phone,
            Role = model.Role,
            Status = model.Status,
            Username = model.Username
        });

        return await GetAsync(id);
    }

    public async Task<UserDto?> GetAsync(int id)
     => await usersRepository.GetAsync<UserDto>(id);

    public async Task<bool> DeleteAsync(int id)
     => await usersRepository.DeleteAsync(id);

    public async Task<IPagedList<UserDto>> GetPagedListAsync(int page, int pageSize, string order)
        => await usersRepository.GetPagedListAsync<UserDto>(page, pageSize, order);

    public async Task<string> ValidateLogin(UserLoginDto model)
    {
        var user = await usersRepository.GetAsync<UserDto>(model.Username, CryptoHelper.Encrypt(model.Password));

        if (user is null)
            throw new CustomException("UserNotFound", "User not found", "No user found with these credentials");

        return TokenHelper.GenerateToken(user);
    }
}