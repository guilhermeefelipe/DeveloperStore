using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.DbContexts;
using DeveloperStore.Repositories;
using DeveloperStore;

namespace DeveloperStore.Repositories.Users;

public interface IUsersRepository
{
    Task<int> CreateAsync(object data);
    Task<bool> DeleteAsync(int id);
    Task UpdateAsync(int id, object data);
    Task<T?> GetAsync<T>(int id) where T : class;
    Task<IPagedList<T>> GetPagedListAsync<T>(int page, int pageSize, string order);
}

public class UsersRepository : IUsersRepository
{
    private readonly IExtendedContext<DeveloperStoreDbContext> context;
    public DeveloperStoreDbContext dbContext { get; }

    public UsersRepository(IExtendedContext<DeveloperStoreDbContext> context, DeveloperStoreDbContext dbContext)
    {
        this.context = context;
        this.dbContext = dbContext;
    }

    public async Task<T?> GetAsync<T>(int id) where T : class
        => await context.GetAsync<User, T>(id);

    public async Task<IPagedList<T>> GetPagedListAsync<T>(int page, int pageSize, string order)
        => await context.GetPagedListAsync<User, T>(page, pageSize, order);

    public async Task<int> CreateAsync(object data)
        => await context.CreateAsync<User>(data);

    public async Task UpdateAsync(int id, object data)
        => await context.UpdateAsync<User>(id, data);

    public async Task<bool> DeleteAsync(int id)
        => await context.DeleteAsync<User>(id);
}