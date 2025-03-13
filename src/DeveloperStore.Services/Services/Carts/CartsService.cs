using DeveloperStore.Domain.Dto.Cart;
using DeveloperStore.Domain.Dto.Product;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.Products;
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
    private readonly ICartsRepository cartsRepository;
    private readonly IUsersService usersService;

    public CartsService(ICartsRepository CartsRepository,
                        IUsersService usersService)
    {
        this.cartsRepository = CartsRepository;
        this.usersService = usersService;
    }

    public async Task<CartDto?> CreateAsync(CartCreateEditRequestDto model)
    {      
        var user = await usersService.GetAsync(model.UserId);

        if (user is null)
            throw new CustomException("ResourceNotFound", "User not found", $"The user with ID {model.UserId} does not exist in our database");

        var productsJson = JsonSerializer.Serialize(model.ProductsList);

        var cartId = await cartsRepository.CreateAsync(new CartCreateEditDto
        {
            Date = DateTime.Now.ToString(),
            Products = JsonSerializer.Serialize(model.ProductsList),
            UserId = model.UserId
        });

        return await GetAsync(cartId);
    }
    public async Task<CartDto?> UpdateAsync(int id, CartCreateEditRequestDto model)
    {
        var cart = await cartsRepository.GetAsync<CartDto>(id);

        if (cart is null)
            throw new CustomException("ResourceNotFound", "Cart not found", $"The cart with ID {id} does not exist in our database");

        var user = await usersService.GetAsync(model.UserId);

        if (user is null)
            throw new CustomException("ResourceNotFound", "User not found", $"The user with ID {id} does not exist in our database");

        var productsJson = JsonSerializer.Serialize(model.ProductsList);

        await cartsRepository.UpdateAsync(id, new CartCreateEditDto
        {
            Date = model.Date,
            Products = productsJson,
            UserId = model.UserId
        });

        return await GetAsync(id);
    }

    public async Task<CartDto?> GetAsync(int id)
    {
        var cart = await cartsRepository.GetAsync<CartCreateEditDto>(id);

        if (cart is null)
            throw new CustomException("ResourceNotFound", "Cart not found", $"The cart with ID {id} does not exist in our database");

        return new CartDto
        {
            Date = cart.Date,
            Id = id,
            ProductList = JsonSerializer.Deserialize<List<ProductCartDto>>(cart.Products),
            UserId = cart.UserId
        };
    }
      
    public async Task<bool> DeleteAsync(int id)
    {
        var cart = await cartsRepository.GetAsync<CartCreateEditDto>(id);

        if (cart is null)
            throw new CustomException("ResourceNotFound", "Cart not found", $"The cart with ID {id} does not exist in our database");

        return await cartsRepository.DeleteAsync(id);
    }

    public async Task<IPagedList<CartDto>> GetPagedListAsync(int page, int pageSize, string order)
    {
        var carts = await cartsRepository.GetPagedListAsync<CartDto>(page, pageSize, order);

        foreach (var item in carts.Items)
        {
            var cart = await cartsRepository.GetAsync<CartCreateEditDto>(item.Id);

            if (cart is null)
                continue;

            item.ProductList = JsonSerializer.Deserialize<List<ProductCartDto>>(cart!.Products);
        }

        return carts;
    }
}