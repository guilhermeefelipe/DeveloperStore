using DeveloperStore.Domain.Dto.Cart;
using DeveloperStore.Services.Carts;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.UI.Controllers;

[Route("carts")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly ICartsService CartsService;

    public CartsController(ICartsService CartsService)
    {
        this.CartsService = CartsService;
    }

    [HttpPost()]
    public async Task<ActionResult> CreateAsync([FromBody] CartCreateEditRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var Cart = await CartsService.CreateAsync(request);

        if (Cart is null)
            return BadRequest("Erro ao criar o usuario!");

        return Ok(Cart);
    }

    [HttpGet()]
    public async Task<ActionResult> GetPagedListAsync(int page = 1, int size = 10, string order = "id desc")
    {
        var list = await CartsService.GetPagedListAsync(page, size, order);

        if (!list.Items.Any())
            return NotFound();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(int id)
    {
        var Cart = await CartsService.GetAsync(id);

        if (Cart is null)
            return NotFound();

        return Ok(Cart);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] CartCreateEditRequestDto request)
    {
        var Cart = await CartsService.UpdateAsync(id, request);

        if (Cart is null)
            return NotFound();

        return Ok(Cart);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await CartsService.DeleteAsync(id);

        return Ok();
    }
}