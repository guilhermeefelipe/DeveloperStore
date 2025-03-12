using DeveloperStore.Domain.Dto.Cart;
using DeveloperStore.Domain.Dto.Product;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.Repositories.Carts;
using DeveloperStore.Services.Raties;
using DeveloperStore.Services.Services;
using DeveloperStore.Services.Users;
using System.Text.Json;

namespace DeveloperStore.Services.Carts;

public interface ICartsService
{
    Task<CartDto?> CreateAsync(CartCreateEditRequestDto model);
    Task<CartDto?> UpdateAsync(int id, CartCreateEditRequestDto model);
    Task<bool> DeleteAsync(int id);
    Task<CartDto?> GetAsync(int id);
    Task<IPagedList<CartDto>> GetPagedListAsync(int page, int pageSize, string order);
}

public class CartsService : ICartsService
{
    private readonly ICartsRepository CartsRepository;
    private readonly IUsersService usersService;

    public CartsService(ICartsRepository CartsRepository,
                           IUsersService usersService)
    {
        this.CartsRepository = CartsRepository;
        this.usersService = usersService;
    }

    public async Task<CartDto?> CreateAsync(CartCreateEditRequestDto model)
    {
        if (model == null)
            throw new CustomException("InvalidRequest", "Request is invalid", "Request is not in the expected standard");
        
        var user = await usersService.GetAsync(model.UserId);

        if (user == null)
            throw new CustomException("UserNotFound", "User not found", $"No Cart found with these id:{model.UserId}");

        var productsJson = JsonSerializer.Serialize(model.ProductsList);

        var CartId = await CartsRepository.CreateAsync(new CartCreateEditDto
        {
            Date = DateTime.Now.ToString(),
            Products = JsonSerializer.Serialize(model.ProductsList),
            UserId = model.UserId
        });

        return await GetAsync(CartId);
    }
    public async Task<CartDto?> UpdateAsync(int id, CartCreateEditRequestDto model)
    {
        if (model == null)
            throw new CustomException("InvalidRequest", "Request is invalid", "Request is not in the expected standard");

        var cart = await CartsRepository.GetAsync<CartDto>(id);

        if (cart == null)
            throw new CustomException("CartNotFound", "Cart not found", $"No Cart found with these id:{id}");

        var user = await usersService.GetAsync(model.UserId);

        if (user == null)
            throw new CustomException("UserNotFound", "User not found", $"No Cart found with these id:{model.UserId}");

        var productsJson = JsonSerializer.Serialize(model.ProductsList);

        await CartsRepository.UpdateAsync(id, new CartCreateEditDto
        {
            Date = model.Date,
            Products = productsJson,
            UserId = model.UserId
        });

        return await GetAsync(id);
    }

    public async Task<CartDto?> GetAsync(int id)
    {
        var cart = await CartsRepository.GetAsync<CartCreateEditDto>(id);

        if (cart == null)
            throw new CustomException("CartNotFound", "Cart not found", $"No Cart found with these id:{id}");

        return new CartDto
        {
            Date = cart.Date,
            Id = id,
            ProductList = JsonSerializer.Deserialize<List<ProductCartDto>>(cart.Products),
            UserId= cart.UserId
        };
    }
      

    public async Task<bool> DeleteAsync(int id)
     => await CartsRepository.DeleteAsync(id);

    public async Task<IPagedList<CartDto>> GetPagedListAsync(int page, int pageSize, string order)
    {
        var carts = await CartsRepository.GetPagedListAsync<CartDto>(page, pageSize, order);

        foreach (var item in carts.Items)
        {
            var cart = await CartsRepository.GetAsync<CartCreateEditDto>(item.Id);

            if (cart is null)
                continue;

            item.ProductList = JsonSerializer.Deserialize<List<ProductCartDto>>(cart!.Products);
        }

        return carts;
    }
}