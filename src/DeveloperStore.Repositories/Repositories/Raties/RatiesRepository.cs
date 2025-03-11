using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Repositories.Raties;

public interface IRatiesRepository
{
    Task<int> CreateAsync(object data);
    Task<bool> DeleteAsync(int id);
    Task UpdateAsync(int id, object data);
    Task<T?> GetAsync<T>(int id) where T : class;
}

public class RatiesRepository : IRatiesRepository
{
    private readonly IExtendedContext<DeveloperStoreDbContext> context;
    public DeveloperStoreDbContext dbContext { get; }

    public RatiesRepository(IExtendedContext<DeveloperStoreDbContext> context, DeveloperStoreDbContext dbContext)
    {
        this.context = context;
        this.dbContext = dbContext;
    }

    public async Task<T?> GetAsync<T>(int id) where T : class
        => await context.GetAsync<Rating, T>(id);
    public async Task<int> CreateAsync(object data)
        => await context.CreateAsync<Rating>(data);

    public async Task UpdateAsync(int id, object data)
        => await context.UpdateAsync<Rating>(id, data);

    public async Task<bool> DeleteAsync(int id)
        => await context.DeleteAsync<Rating>(id);
}