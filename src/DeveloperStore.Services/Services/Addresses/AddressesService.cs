using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Domain.Dto.Rating;
using DeveloperStore.Repositories.Addresses;
using DeveloperStore.Repositories.Raties;

namespace DeveloperStore.Services.Addresses;

public interface IAddressesService
{
    Task<int> CreateAsync(AddressCreateEditDto model);
    Task UpdateAsync(int id, AddressCreateEditDto model);

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

    public async Task UpdateAsync(int id, AddressCreateEditDto model)
        => await addressesRepository.UpdateAsync(id, model);
}