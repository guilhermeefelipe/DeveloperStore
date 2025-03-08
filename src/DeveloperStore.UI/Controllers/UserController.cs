using DeveloperStore.Domain.Dto.Users;
using DeveloperStore.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DeveloperStore.UI.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService usersService;

    public UsersController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpPost()]
    public async Task<ActionResult> CreateAsync([FromBody] UserCreateEditRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await usersService.CreateAsync(request);

        if (user is null)
            return BadRequest("Erro ao criar o usuario!");

        return Ok(user);
    }

    [HttpGet()]
    public async Task<ActionResult> GetPagedListAsync(int page = 1, int size = 10, string order = "id desc")
    {
        var list = await usersService.GetPagedListAsync(page, size, order);
        
        if (!list.Items.Any())
            return NotFound();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(int id)
    {
        var user = await usersService.GetAsync(id);

        if (user != null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] UserCreateEditRequestDto request)
    {
        var user = await usersService.UpdateAsync(id, request);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await usersService.DeleteAsync(id);

        return Ok();
    }
}