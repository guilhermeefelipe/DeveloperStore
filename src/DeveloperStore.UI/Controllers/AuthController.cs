using DeveloperStore.Domain.Dto.Cart;
using DeveloperStore.Domain.Dto.User;
using DeveloperStore.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DeveloperStore.UI.Controllers;

[Route("auth/login")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUsersService usersService;

    public AuthController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpPost()]
    [SwaggerOperation(Summary = "Authenticate a user")]

    public async Task<ActionResult> CreateAsync([FromBody] UserLoginDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = await usersService.ValidateLogin(request);

        return Ok(token);
    }
}