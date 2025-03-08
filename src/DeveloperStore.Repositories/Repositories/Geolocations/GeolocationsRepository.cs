using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Repositories.Geolocations;

public interface IGeolocationsRepository
{
    Task<int> CreateAsync(object data);
    Task<bool> DeleteAsync(int id);
    Task UpdateAsync(int id, object data);
    Task<T?> GetAsync<T>(int id) where T : class;
}

public class GeolocationsRepository : IGeolocationsRepository
{
    private readonly IExtendedContext<DeveloperStoreDbContext> context;
    public DeveloperStoreDbContext dbContext { get; }

    public GeolocationsRepository(IExtendedContext<DeveloperStoreDbContext> context, DeveloperStoreDbContext dbContext)
    {
        this.context = context;
        this.dbContext = dbContext;
    }

    public async Task<T?> GetAsync<T>(int id) where T : class
        => await context.GetAsync<Geolocation, T>(id);

    public async Task<int> CreateAsync(object data)
        => await context.CreateAsync<Geolocation>(data);

    public async Task UpdateAsync(int id, object data)
        => await context.UpdateAsync<Geolocation>(id, data);

    public async Task<bool> DeleteAsync(int id)
        => await context.DeleteAsync<Geolocation>(id);
}