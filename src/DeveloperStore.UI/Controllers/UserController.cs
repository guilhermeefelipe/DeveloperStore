using DeveloperStore.Domain.Dto.Users;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Services.Services;
using DeveloperStore.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DeveloperStore.UI.Controllers;

[Route("users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService usersService;

    public UsersController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpGet()]
    [SwaggerOperation(Summary = "Retrieve a list of all users")]
    public async Task<ActionResult> GetPagedListAsync(int page = 1, int size = 10, string order = "id desc")
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var list = await usersService.GetPagedListAsync(page, size, order);

        return Ok(list);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieve a specific user by ID")]
    public async Task<ActionResult> GetAsync(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await usersService.GetAsync(id);

        return Ok(user);
    }

    [HttpPost()]
    [SwaggerOperation(Summary = "Add a new user")]
    public async Task<ActionResult> CreateAsync([FromBody] UserCreateEditRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await usersService.CreateAsync(request);

        if (user is null)
            throw new CustomException("CreateError", "Error creating user", "An error occurred while creating the user");

        return Ok(user);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a specific user")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] UserCreateEditRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await usersService.UpdateAsync(id, request);

        if (user is null)
            throw new CustomException("UpdateError", "Error updating user", "An error occurred while updating the user");

        return Ok(user);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a specific user")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await usersService.DeleteAsync(id);

        return Ok("User deleted");
    }
}