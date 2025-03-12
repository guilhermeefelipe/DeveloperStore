using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Domain.Dto.Product;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.Addresses;
using DeveloperStore.Repositories.Geolocations;
using DeveloperStore.Repositories.Names;
using DeveloperStore.Repositories.Products;
using DeveloperStore.Repositories.Raties;
using DeveloperStore.Services.Raties;
using DeveloperStore.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Services.Products;

public interface IProductsService
{
    Task<ProductCompleteDto?> CreateAsync(ProductCreateEditRequestDto model);
    Task<ProductCompleteDto?> UpdateAsync(int id, ProductCreateEditRequestDto model);
    Task<bool> DeleteAsync(int id);
    Task<ProductCompleteDto?> GetAsync(int id);
    Task<IPagedList<ProductDto>> GetPagedListAsync(int page, int pageSize, string order);
    Task<IPagedList<ProductDto>> GetPagedListAsync(int page, int pageSize, string order, string where);
    Task<IEnumerable<string>> GetCategoriesListAsync();
}

public class ProductsService : IProductsService
{
    private readonly IProductsRepository ProductsRepository;
    private readonly IRatiesService ratiesService;

    public ProductsService(IProductsRepository ProductsRepository,
                           IRatiesService ratiesService)
    {
        this.ProductsRepository = ProductsRepository;
        this.ratiesService = ratiesService;
    }

    public async Task<ProductCompleteDto?> CreateAsync(ProductCreateEditRequestDto model)
    {
        if (model == null)
            throw new CustomException("InvalidRequest", "Request is invalid", "Request is not in the expected standard");

        var ratingId = await ratiesService.CreateAsync(model.Rating);

        var productId = await ProductsRepository.CreateAsync(new ProductCreateEditDto
        {
            Category = model.Category,
            Description = model.Description,
            Image = model.Image,
            Price = model.Price,
            Title = model.Title,
            RatingId = ratingId,
        });

        return await GetAsync(productId);
    }
    public async Task<ProductCompleteDto?> UpdateAsync(int id, ProductCreateEditRequestDto model)
    {
        if (model == null)
            throw new CustomException("InvalidRequest", "Request is invalid", "Request is not in the expected standard");

        var Product = await ProductsRepository.GetAsync<ProductCompleteDto>(id);

        if (Product == null)
            throw new CustomException("ProductNotFound", "Product not found", $"No Product found with these id:{id}");

        await ratiesService.UpdateAsync(Product.Rating.Id, model.Rating);

        await ProductsRepository.UpdateAsync(id, new ProductCreateEditDto
        {
            Category = model.Category,
            Description = model.Description,
            Image = model.Image,
            Price = model.Price,
            Title = model.Title,
            RatingId = Product.Rating.Id
        });

        return await GetAsync(id);
    }

    public async Task<ProductCompleteDto?> GetAsync(int id)
     => await ProductsRepository.GetAsync<ProductCompleteDto>(id);

    public async Task<bool> DeleteAsync(int id)
     => await ProductsRepository.DeleteAsync(id);

    public async Task<IPagedList<ProductDto>> GetPagedListAsync(int page, int pageSize, string order)
        => await ProductsRepository.GetPagedListAsync<ProductDto>(page, pageSize, order);
    public async Task<IPagedList<ProductDto>> GetPagedListAsync(int page, int pageSize, string order, string where)
        => await ProductsRepository.GetPagedListAsync<ProductDto>(page, pageSize, order, where);

    public async Task<IEnumerable<string>> GetCategoriesListAsync()
        => await ProductsRepository.GetCategoriesListAsync();
}