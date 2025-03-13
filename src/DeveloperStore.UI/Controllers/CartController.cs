using DeveloperStore.Domain.Dto.Cart;
using DeveloperStore.Services.Carts;
using DeveloperStore.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.UI.Controllers;

[Route("carts")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly ICartsService cartsService;

    public CartsController(ICartsService cartsService)
    {
        this.cartsService = cartsService;
    }

    [HttpGet()]
    public async Task<ActionResult> GetPagedListAsync(int page = 1, int size = 10, string order = "id desc")
    {
        var list = await cartsService.GetPagedListAsync(page, size, order);

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cart = await cartsService.GetAsync(id);

        return Ok(cart);
    }

    [HttpPost()]
    public async Task<ActionResult> CreateAsync([FromBody] CartCreateEditRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cart = await cartsService.CreateAsync(request);

        if (cart is null)
            throw new CustomException("CreateError", "Error creating cart", "An error occurred while creating the cart");

        return Ok(cart);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] CartCreateEditRequestDto request)
    {
        var Cart = await cartsService.UpdateAsync(id, request);

        if (Cart is null)
            throw new CustomException("UpdateError", "Error updating cart", "An error occurred while updating the cart");

        return Ok(Cart);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await cartsService.DeleteAsync(id);

        return Ok("Cart deleted");
    }
}