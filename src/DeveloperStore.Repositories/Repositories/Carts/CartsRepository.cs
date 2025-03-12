using DeveloperStore.Repositories;
using DeveloperStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DeveloperStore.Repositories.DbContexts;
using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace DeveloperStore.Repositories.Repositories.Carts;

public interface ICartsRepository
{
    Task<int> CreateAsync(object data);
    Task<bool> DeleteAsync(int id);
    Task UpdateAsync(int id, object data);
    Task<T?> GetAsync<T>(int id) where T : class;
    Task<IPagedList<T>> GetPagedListAsync<T>(int page, int pageSize, string order);
    Task<string> GetProductCartJsonAsync(int id);
}

public class CartsRepository : ICartsRepository
{
    private readonly IExtendedContext<DeveloperStoreDbContext> context;
    public DeveloperStoreDbContext dbContext { get; }

    public CartsRepository(IExtendedContext<DeveloperStoreDbContext> context, DeveloperStoreDbContext dbContext)
    {
        this.context = context;
        this.dbContext = dbContext;
    }

    public async Task<IPagedList<T>> GetPagedListAsync<T>(int page, int pageSize, string order)
    => await context.GetPagedListAsync<Cart, T>(page, pageSize, order);

    public async Task<T?> GetAsync<T>(int id) where T : class
        => await context.GetAsync<Cart, T>(id);

    public async Task<int> CreateAsync(object data)
        => await context.CreateAsync<Cart>(data);

    public async Task UpdateAsync(int id, object data)
        => await context.UpdateAsync<Cart>(id, data);

    public async Task<bool> DeleteAsync(int id)
        => await context.DeleteAsync<Cart>(id);

    public async Task<string> GetProductCartJsonAsync(int id)
    {
        return await dbContext.Cart
                   .AsNoTracking()
                   .Where(i => i.Id == id)
                   .Select(i => i.Products)
                   .FirstOrDefaultAsync() ?? string.Empty;
    }

}