﻿using DeveloperStore.Domain.Dto.Product;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Services.Products;
using DeveloperStore.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DeveloperStore.UI.Controllers;


[Route("products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductsService productsService;

    public ProductsController(IProductsService productsService)
    {
        this.productsService = productsService;
    }

    [HttpGet()]
    [SwaggerOperation(Summary = "Retrieve a list of all products")]
    public async Task<ActionResult> GetPagedListAsync(int page = 1, int size = 10, string order = "id desc")
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var list = await productsService.GetPagedListAsync(page, size, order);

        return Ok(list);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieve a specific product by ID")]
    public async Task<ActionResult> GetAsync(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await productsService.GetAsync(id);

        return Ok(product);
    }

    [HttpPost()]
    [SwaggerOperation(Summary = "Add a new product")]
    public async Task<ActionResult> CreateAsync([FromBody] ProductCreateEditRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await productsService.CreateAsync(request);

        if (product is null)
            throw new CustomException("CreateError", "Error creating product", "An error occurred while creating the product");

        return Ok(product);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a specific product")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] ProductCreateEditRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await productsService.UpdateAsync(id, request);

        if (product is null)
            throw new CustomException("UpdateError", "Error updating product", "An error occurred while updating the product");

        return Ok(product);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a specific product")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await productsService.DeleteAsync(id);

        return Ok("Product deleted");
    }

    [HttpGet("categories")]
    [SwaggerOperation(Summary = "Retrieve all product categories")]
    public async Task<ActionResult> GetCategoriesAsync()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var categories = await productsService.GetCategoriesListAsync();

        return Ok(categories);
    }

    [HttpGet("category/{category}")]
    [SwaggerOperation(Summary = "Retrieve products in a specific category")]
    public async Task<ActionResult> GetCategoriesAsync(string category, int page = 1, int size = 10, string order = "id desc")
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var list = await productsService.GetPagedListAsync(page, size, order, category);

        return Ok(list);
    }
}