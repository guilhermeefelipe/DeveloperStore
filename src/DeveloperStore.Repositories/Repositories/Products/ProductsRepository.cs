using DeveloperStore.Domain.Dto.Product;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Xml;

namespace DeveloperStore.Repositories.Products;

public interface IProductsRepository
{
    Task<int> CreateAsync(object data);
    Task<bool> DeleteAsync(int id);
    Task UpdateAsync(int id, object data);
    Task<T?> GetAsync<T>(int id) where T : class;
    Task<IPagedList<T>> GetPagedListAsync<T>(int page, int pageSize, string order);
    Task<IPagedList<T>> GetPagedListAsync<T>(int page, int pageSize, string order, string where);
    Task<IEnumerable<string>> GetCategoriesListAsync();

}

public class ProductsRepository : IProductsRepository
{
    private readonly IExtendedContext<DeveloperStoreDbContext> context;
    public DeveloperStoreDbContext dbContext { get; }

    public ProductsRepository(IExtendedContext<DeveloperStoreDbContext> context, DeveloperStoreDbContext dbContext)
    {
        this.context = context;
        this.dbContext = dbContext;
    }

    public async Task<T?> GetAsync<T>(int id) where T : class
        => await context.GetAsync<Product, T>(id);

    public async Task<IPagedList<T>> GetPagedListAsync<T>(int page, int pageSize, string order)
    => await context.GetPagedListAsync<Product, T>(page, pageSize, order);

    public async Task<IPagedList<T>> GetPagedListAsync<T>(int page, int pageSize, string order, string where)
    {
        Expression<Func<Product, bool>> whereCondition = x => x.Category.Contains(where);
        return await context.GetPagedListAsync<Product, T>(page, pageSize, order);
    }
    public async Task<int> CreateAsync(object data)
        => await context.CreateAsync<Product>(data);

    public async Task UpdateAsync(int id, object data)
        => await context.UpdateAsync<Product>(id, data);

    public async Task<bool> DeleteAsync(int id)
        => await context.DeleteAsync<Product>(id);

    public async Task<IEnumerable<string>> GetCategoriesListAsync()
    {
        return await dbContext.Product
            .AsNoTracking()
            .Select(x => x.Category)
            .Distinct()
            .ToListAsync();
    }
}