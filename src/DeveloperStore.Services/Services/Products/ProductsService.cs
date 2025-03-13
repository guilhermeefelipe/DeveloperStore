using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Domain.Dto.Product;
using DeveloperStore.Domain.Dto.Users;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.Addresses;
using DeveloperStore.Repositories.Geolocations;
using DeveloperStore.Repositories.Names;
using DeveloperStore.Repositories.Products;
using DeveloperStore.Repositories.Raties;
using DeveloperStore.Repositories.Users;
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
    Task<ProductDto?> CreateAsync(ProductCreateEditRequestDto model);
    Task<ProductDto?> UpdateAsync(int id, ProductCreateEditRequestDto model);
    Task<bool> DeleteAsync(int id);
    Task<ProductDto?> GetAsync(int id);
    Task<IPagedList<ProductDto>> GetPagedListAsync(int page, int pageSize, string order);
    Task<IPagedList<ProductDto>> GetPagedListAsync(int page, int pageSize, string order, string where);
    Task<IEnumerable<string>> GetCategoriesListAsync();
}

public class ProductsService : IProductsService
{
    private readonly IProductsRepository productsRepository;
    private readonly IRatiesService ratiesService;

    public ProductsService(IProductsRepository productsRepository,
                           IRatiesService ratiesService)
    {
        this.productsRepository = productsRepository;
        this.ratiesService = ratiesService;
    }

    public async Task<ProductDto?> CreateAsync(ProductCreateEditRequestDto model)
    {
        var ratingId = await ratiesService.CreateAsync(model.Rating);

        var productId = await productsRepository.CreateAsync(new ProductCreateEditDto
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
    public async Task<ProductDto?> UpdateAsync(int id, ProductCreateEditRequestDto model)
    {
        var product = await productsRepository.GetAsync<ProductCompleteDto>(id);

        if (product is null)
            throw new CustomException("ResourceNotFound", "Product not found", $"The product with ID {id} does not exist in our database");

        await ratiesService.UpdateAsync(product.Rating.Id, model.Rating);

        await productsRepository.UpdateAsync(id, new ProductCreateEditDto
        {
            Category = model.Category,
            Description = model.Description,
            Image = model.Image,
            Price = model.Price,
            Title = model.Title,
            RatingId = product.Rating.Id
        });

        return await GetAsync(id);
    }

    public async Task<ProductDto?> GetAsync(int id)
    {
        var product = await productsRepository.GetAsync<ProductDto>(id);

        if (product is null)
            throw new CustomException("ResourceNotFound", "Product not found", $"The product with ID {id} does not exist in our database");

        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await productsRepository.GetAsync<ProductCompleteDto>(id);

        if (product is null)
            throw new CustomException("ResourceNotFound", "Product not found", $"The product with ID {id} does not exist in our database");

        return await productsRepository.DeleteAsync(id);
    }

    public async Task<IPagedList<ProductDto>> GetPagedListAsync(int page, int pageSize, string order)
        => await productsRepository.GetPagedListAsync<ProductDto>(page, pageSize, order);
    public async Task<IPagedList<ProductDto>> GetPagedListAsync(int page, int pageSize, string order, string where)
        => await productsRepository.GetPagedListAsync<ProductDto>(page, pageSize, order, where);

    public async Task<IEnumerable<string>> GetCategoriesListAsync()
        => await productsRepository.GetCategoriesListAsync();
}