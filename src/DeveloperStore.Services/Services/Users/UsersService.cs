using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Domain.Dto.User;
using DeveloperStore.Domain.Dto.Users;
using DeveloperStore.Repositories.Users;
using DeveloperStore.Services.Addresses;
using DeveloperStore.Services.Geolocations;
using DeveloperStore.Services.Names;
using DeveloperStore.Services.Services;

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
    private readonly INamesService namesService;
    private readonly IGeolocationsService geolocationsService;
    private readonly IAddressesService addressesService;

    public UsersService(IUsersRepository usersRepository,
                        INamesService namesService,
                        IGeolocationsService geolocationsService,
                        IAddressesService addressesService)
    {
        this.usersRepository = usersRepository;
        this.namesService = namesService;
        this.geolocationsService = geolocationsService;
        this.addressesService = addressesService;
    }

    public async Task<UserDto?> CreateAsync(UserCreateEditRequestDto model)
    {
        EnumValidator.ValidateRole(model.Role);
        EnumValidator.ValidateStatus(model.Status);

        var geolocationId = await geolocationsService.CreateAsync(model.Address.Geolocation);

        var addressId = await addressesService.CreateAsync(new AddressCreateEditDto
        {  
            City = model.Address.City,
            GeolocationId = geolocationId,
            Number = model.Address.Number,
            Street = model.Address.Street,
            ZipCode = model.Address.ZipCode
        });

        var nameId = await namesService.CreateAsync(model.Name);

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
        var user = await usersRepository.GetAsync<UserCompleteDto>(id);

        if (user is null)
            throw new CustomException("ResourceNotFound", "User not found", $"The user with ID {id} does not exist in our database");

        EnumValidator.ValidateRole(model.Role);
        EnumValidator.ValidateStatus(model.Status);

        await geolocationsService.UpdateAsync(user.Address.Geolocation.Id, model.Address.Geolocation);

        await addressesService.UpdateAsync(user.Address.Id, new AddressCreateEditDto
        {
            City = model.Address.City,
            GeolocationId = user.Address.Geolocation.Id,
            Number = model.Address.Number,
            Street = model.Address.Street,
            ZipCode = model.Address.ZipCode
        });

        await namesService.UpdateAsync(user.Name.Id, model.Name);

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
    {
        var user = await usersRepository.GetAsync<UserDto>(id);

        if (user is null)
            throw new CustomException("ResourceNotFound", "User not found", $"The user with ID {id} does not exist in our database");

        return user;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await usersRepository.GetAsync<UserCompleteDto>(id);

        if (user is null)
            throw new CustomException("ResourceNotFound", "User not found", $"The user with ID {id} does not exist in our database");

       return await usersRepository.DeleteAsync(id);
    }

    public async Task<IPagedList<UserDto>> GetPagedListAsync(int page, int pageSize, string order)
        => await usersRepository.GetPagedListAsync<UserDto>(page, pageSize, order);

    public async Task<string> ValidateLogin(UserLoginDto model)
    {
        var user = await usersRepository.GetAsync<UserDto>(model.Username, CryptoHelper.Encrypt(model.Password));

        if (user is null)
            throw new CustomException("ResourceNotFound", "User not found", "No user found with these credentials");

        return TokenHelper.GenerateToken(user);
    }
}