using DeveloperStore;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.DbContexts;

namespace DeveloperStore.Repositories.Addresses;

public interface IAddressesRepository
{
    Task<int> CreateAsync(object data);
    Task<bool> DeleteAsync(int id);
    Task UpdateAsync(int id, object data);
    Task<T?> GetAsync<T>(int id) where T : class;
}

public class AddressesRepository : IAddressesRepository
{
    private readonly IExtendedContext<DeveloperStoreDbContext> context;
    public DeveloperStoreDbContext dbContext { get; }

    public AddressesRepository(IExtendedContext<DeveloperStoreDbContext> context, DeveloperStoreDbContext dbContext)
    {
        this.context = context;
        this.dbContext = dbContext;
    }

    public async Task<T?> GetAsync<T>(int id) where T : class
        => await context.GetAsync<Address, T>(id);

    public async Task<int> CreateAsync(object data)
        => await context.CreateAsync<Address>(data);

    public async Task UpdateAsync(int id, object data)
        => await context.UpdateAsync<Address>(id, data);

    public async Task<bool> DeleteAsync(int id)
        => await context.DeleteAsync<Address>(id);
}