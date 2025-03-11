using DeveloperStore.Domain.Dto.Product;
using DeveloperStore.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.UI.Controllers;


[Route("products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductsService ProductsService;

    public ProductsController(IProductsService ProductsService)
    {
        this.ProductsService = ProductsService;
    }

    [HttpPost()]
    public async Task<ActionResult> CreateAsync([FromBody] ProductCreateEditRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var Product = await ProductsService.CreateAsync(request);

        if (Product is null)
            return BadRequest("Erro ao criar o usuario!");

        return Ok(Product);
    }

    [HttpGet()]
    public async Task<ActionResult> GetPagedListAsync(int page = 1, int size = 10, string order = "id desc")
    {
        var list = await ProductsService.GetPagedListAsync(page, size, order);

        if (!list.Items.Any())
            return NotFound();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(int id)
    {
        var Product = await ProductsService.GetAsync(id);

        if (Product is null)
            return NotFound();

        return Ok(Product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] ProductCreateEditRequestDto request)
    {
        var Product = await ProductsService.UpdateAsync(id, request);

        if (Product is null)
            return NotFound();

        return Ok(Product);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await ProductsService.DeleteAsync(id);

        return Ok();
    }

    [HttpGet("categories")]
    public async Task<ActionResult> GetCategoriesAsync()
    {
        var categories = await ProductsService.GetCategoriesListAsync();

        if (categories is null)
            return NotFound();

        return Ok(categories);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult> GetCategoriesAsync(string category, int page = 1, int size = 10, string order = "id desc")
    {
        var list = await ProductsService.GetPagedListAsync(page, size, order, category);

        if (!list.Items.Any())
            return NotFound();

        return Ok(list);
    }
}