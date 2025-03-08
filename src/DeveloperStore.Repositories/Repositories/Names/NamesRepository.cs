using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Repositories.Names;

public interface INamesRepository
{
    Task<int> CreateAsync(object data);
    Task<bool> DeleteAsync(int id);
    Task UpdateAsync(int id, object data);
    Task<T?> GetAsync<T>(int id) where T : class;
}

public class NamesRepository : INamesRepository
{
    private readonly IExtendedContext<DeveloperStoreDbContext> context;
    public DeveloperStoreDbContext dbContext { get; }

    public NamesRepository(IExtendedContext<DeveloperStoreDbContext> context, DeveloperStoreDbContext dbContext)
    {
        this.context = context;
        this.dbContext = dbContext;
    }

    public async Task<T?> GetAsync<T>(int id) where T : class
        => await context.GetAsync<Name, T>(id);
    public async Task<int> CreateAsync(object data)
        => await context.CreateAsync<Name>(data);

    public async Task UpdateAsync(int id, object data)
        => await context.UpdateAsync<Name>(id, data);

    public async Task<bool> DeleteAsync(int id)
        => await context.DeleteAsync<Name>(id);
}