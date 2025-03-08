using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Repositories.Addresses;

namespace DeveloperStore.Services.Addresses;

public interface IAddressesService
{
    Task<int> CreateAsync(AddressCreateEditDto model);
}

public class AddressesService : IAddressesService
{
    private readonly IAddressesRepository addressesRepository;

    public AddressesService(IAddressesRepository addressesRepository)
    {
        this.addressesRepository = addressesRepository;
    }

    public async Task<int> CreateAsync(AddressCreateEditDto model)
        => await addressesRepository.CreateAsync(model);
}